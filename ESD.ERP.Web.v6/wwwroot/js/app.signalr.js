// ========================================================
// SISTEMA DE NOTIFICACIONES Y SONIDO (CAMPANITA)
// ========================================================
var _notificationAudioCtx = null;
var _isSoundEnabled = localStorage.getItem('arco_notification_sound_enabled') !== 'false';
var _prevNotificationTotal = -1;
var _hasInitialLoaded = false;
var _knownNotificationIds = {};

function getNotificationAudioContext() {
   if (!_notificationAudioCtx) {
      var AudioContextClass = window.AudioContext || window.webkitAudioContext;
      if (AudioContextClass) {
         _notificationAudioCtx = new AudioContextClass();
      }
   }
   return _notificationAudioCtx;
}

// Desbloquear AudioContext en la primera interacción del usuario según políticas del navegador
function unlockAudioContext() {
   var ctx = getNotificationAudioContext();
   if (ctx && ctx.state === 'suspended') {
      ctx.resume().catch(function () { });
   }
}
document.addEventListener('click', unlockAudioContext, { once: true, passive: true });
document.addEventListener('keydown', unlockAudioContext, { once: true, passive: true });
document.addEventListener('touchstart', unlockAudioContext, { once: true, passive: true });

function tocarCampanita(ctx) {
   var now = ctx.currentTime;

   // 1. Nota Principal (Campana brillante cristalina ~1320 Hz - Mi / E6)
   var osc1 = ctx.createOscillator();
   var gain1 = ctx.createGain();
   osc1.type = 'sine';
   osc1.frequency.setValueAtTime(1320, now);
   osc1.frequency.exponentialRampToValueAtTime(1318, now + 0.6);
   gain1.gain.setValueAtTime(0, now);
   gain1.gain.linearRampToValueAtTime(0.32, now + 0.015);
   gain1.gain.exponentialRampToValueAtTime(0.0001, now + 0.7);
   osc1.connect(gain1);
   gain1.connect(ctx.destination);
   osc1.start(now);
   osc1.stop(now + 0.75);

   // 2. Armónico Superior / Eco metálico (~2093 Hz - Do / C7), retrasado 75ms
   var osc2 = ctx.createOscillator();
   var gain2 = ctx.createGain();
   osc2.type = 'sine';
   osc2.frequency.setValueAtTime(2093, now + 0.075);
   gain2.gain.setValueAtTime(0, now);
   gain2.gain.setValueAtTime(0, now + 0.075);
   gain2.gain.linearRampToValueAtTime(0.25, now + 0.09);
   gain2.gain.exponentialRampToValueAtTime(0.0001, now + 1.1);
   osc2.connect(gain2);
   gain2.connect(ctx.destination);
   osc2.start(now + 0.075);
   osc2.stop(now + 1.15);

   // 3. Resonancia base de calidez (~880 Hz - La / A5)
   var osc3 = ctx.createOscillator();
   var gain3 = ctx.createGain();
   osc3.type = 'triangle';
   osc3.frequency.setValueAtTime(880, now);
   gain3.gain.setValueAtTime(0, now);
   gain3.gain.linearRampToValueAtTime(0.12, now + 0.02);
   gain3.gain.exponentialRampToValueAtTime(0.0001, now + 0.5);
   osc3.connect(gain3);
   gain3.connect(ctx.destination);
   osc3.start(now);
   osc3.stop(now + 0.55);
}

function reproducirCampanitaNotificacion(force) {
   if (!force && !_isSoundEnabled) return;
   try {
      var ctx = getNotificationAudioContext();
      if (!ctx) return;
      if (ctx.state === 'suspended') {
         ctx.resume().then(function () {
            tocarCampanita(ctx);
         }).catch(function () { });
      } else {
         tocarCampanita(ctx);
      }
   } catch (e) {
      console.warn('Error al reproducir campanita de notificación:', e);
   }
}

// Expuesto globalmente para pruebas y llamadas desde consola u otros módulos
window.reproducirSonidoNotificacion = reproducirCampanitaNotificacion;
window.probarCampanita = function () {
   reproducirCampanitaNotificacion(true);
   animarCampanitaNavbar();
};

function animarCampanitaNavbar() {
   var $icon = $('#app-notification-trigger').find('i.fa-bell');
   if ($icon.length) {
      $icon.removeClass('app-notification-bell-ring');
      setTimeout(function () {
         $icon.addClass('app-notification-bell-ring');
      }, 20);
      setTimeout(function () {
         $icon.removeClass('app-notification-bell-ring');
      }, 1200);
   }
}

function actualizarBotonSonidoUI() {
   var $btn = $('#app-notification-sound-toggle');
   if (!$btn.length) return;
   if (_isSoundEnabled) {
      $btn.removeClass('muted');
      $btn.find('i').removeClass('fa-volume-mute').addClass('fa-volume-up');
      $btn.attr('title', 'Sonido de notificaciones: Activado (clic para silenciar)');
   } else {
      $btn.addClass('muted');
      $btn.find('i').removeClass('fa-volume-up').addClass('fa-volume-mute');
      $btn.attr('title', 'Sonido de notificaciones: Silenciado (clic para activar)');
   }
}

$(document).on('click', '#app-notification-sound-toggle', function (e) {
   e.preventDefault();
   e.stopPropagation();
   _isSoundEnabled = !_isSoundEnabled;
   localStorage.setItem('arco_notification_sound_enabled', _isSoundEnabled ? 'true' : 'false');
   actualizarBotonSonidoUI();
   if (_isSoundEnabled) {
      reproducirCampanitaNotificacion(true);
   }
});

$(function () {
   actualizarBotonSonidoUI();
});

// ========================================================
// CARGA Y GESTIÓN DE NOTIFICACIONES
// ========================================================
function listarNotificaciones() {
   $(this).getAction({
      urlAction: getBaseUrl() + '/ListarNotificaciones',
      showLoading: false,
      hideLoading: false,
      onSuccess: function (response) {
         if (response.success) {
            buildNotificationDropdown(response.data);
         } else {
            msgAlert(msgAlertTypeConstant.INFORMATION, response.message);
         }
      }
   });
}

function buildNotificationDropdown(data) {
   var dropdown = $('#app-notification-dropdown');
   var notifications = data && Array.isArray(data.list) ? data.list : [];
   var total = data && Number(data.total) ? Number(data.total) : notifications.length;

   // Detectar si llegaron notificaciones nuevas (después de la carga inicial)
   var hayNuevas = false;
   if (_hasInitialLoaded) {
      if (total > _prevNotificationTotal) {
         hayNuevas = true;
      } else {
         for (var i = 0; i < notifications.length; i++) {
            var nId = notifications[i].id || notifications[i].notificacionPushId;
            if (nId && !_knownNotificationIds[nId]) {
               hayNuevas = true;
               break;
            }
         }
      }
   }

   // Actualizar el mapa de IDs conocidos
   _knownNotificationIds = {};
   notifications.forEach(function (item) {
      var id = item.id || item.notificacionPushId;
      if (id) _knownNotificationIds[id] = true;
   });

   _prevNotificationTotal = total;

   if (!_hasInitialLoaded) {
      _hasInitialLoaded = true;
   } else if (hayNuevas) {
      reproducirCampanitaNotificacion();
      animarCampanitaNavbar();
   }

   dropdown.empty();
   $('#app-notification-icon').text(total > 0 ? total : '');
   $('#app-notification-total').text(total);
   $('#app-notification-clear-all').prop('disabled', notifications.length === 0);
   $('#app-notification-empty').prop('hidden', notifications.length > 0);

   notifications.forEach(function (item) {
      var id = item.id || item.notificacionPushId;
      var title = item.titulo || 'Notificación';
      var isCalendar = item.tipo === 'CALENDARIO' || (item.eventoId && Number(item.eventoId) > 0);
      var card = $('<article>').addClass('app-notification-card').attr('role', 'listitem');

      if (isCalendar) {
         card.addClass('app-notification-calendar-item');
         card.attr('data-evento-id', item.eventoId);
         if (item.areaId) card.attr('data-area-id', item.areaId);
         card.attr('title', 'Clic para abrir este evento en el Calendario');
      }

      var iconClass = isCalendar ? 'far fa-calendar-alt' : 'far fa-bell';
      var icon = $('<span>').addClass('app-notification-card-icon').append(
         $('<i>').addClass(iconClass).attr('aria-hidden', 'true')
      );
      var content = $('<div>').addClass('app-notification-card-content');
      var actions = $('<div>').addClass('app-notification-card-actions');
      var deleteButton = $('<button>')
         .attr('type', 'button')
         .attr('aria-label', 'Eliminar notificación')
         .attr('title', 'Eliminar notificación')
         .attr('data-notification-id', id)
         .addClass('app-notification-delete')
         .append($('<i>').addClass('fas fa-times').attr('aria-hidden', 'true'));

      $('<h3>').addClass('app-notification-card-title').text(title).appendTo(content);
      $('<p>').addClass('app-notification-card-description').text(item.descripcion || '').appendTo(content);

      var footer = $('<div>').addClass('app-notification-card-footer');
      $('<time>').addClass('app-notification-card-time').text(item.fecha || '').appendTo(footer);
      if (isCalendar) {
         $('<span>').addClass('app-notification-card-badge')
            .html('<i class="fas fa-calendar-check mr-1"></i>Abrir evento')
            .appendTo(footer);
      }
      footer.appendTo(content);

      actions.append(deleteButton);
      card.append(icon, content, actions);
      dropdown.append(card);
   });
}

function confirmarVaciarNotificaciones() {
   var darkMode = document.body.classList.contains('dark-mode');
   var dialog = window.Swal
      ? Swal.fire({
         title: '¿Vaciar la bandeja?',
         text: 'Se eliminarán las notificaciones de hoy.',
         icon: 'warning',
         showCancelButton: true,
         confirmButtonText: 'Sí, vaciar',
         cancelButtonText: 'Conservar',
         reverseButtons: true,
         background: darkMode ? '#202833' : '#ffffff',
         color: darkMode ? '#edf2f7' : '#1e293b',
         confirmButtonColor: '#dc3545',
         cancelButtonColor: darkMode ? '#556273' : '#64748b'
      })
      : Promise.resolve({ isConfirmed: window.confirm('¿Eliminar las notificaciones de hoy?') });

   dialog.then(function (result) {
      if (!result.isConfirmed) return;

      var clearButton = $('#app-notification-clear-all');
      clearButton.prop('disabled', true);
      $.ajax({
         url: getBaseUrl() + '/VaciarNotificaciones',
         type: 'POST',
         success: function (response) {
            if (response.success) {
               listarNotificaciones();
               msgAlert(msgAlertTypeConstant.SUCCESS, response.message || 'Bandeja vaciada.');
            } else {
               clearButton.prop('disabled', false);
               listarNotificaciones();
               msgAlert(msgAlertTypeConstant.ERROR, response.message || 'No se pudo vaciar la bandeja.');
            }
         },
         error: function () {
            clearButton.prop('disabled', false);
            listarNotificaciones();
            msgAlert(msgAlertTypeConstant.ERROR, 'No se pudo vaciar la bandeja.');
         }
      });
   });
}

$(document)
   .off('click.appNotifications', '.app-notification-delete')
   .on('click.appNotifications', '.app-notification-delete', function (event) {
      event.preventDefault();
      event.stopPropagation();

      var button = $(this);
      var notificationId = Number(button.attr('data-notification-id'));
      if (!notificationId || button.prop('disabled')) return;

      button.prop('disabled', true);
      $.ajax({
         url: getBaseUrl() + '/EliminarNotificacion',
         type: 'POST',
         data: { id: notificationId },
         success: function (response) {
            if (response.success) {
               listarNotificaciones();
               msgAlert(msgAlertTypeConstant.SUCCESS, response.message || 'Notificación eliminada.');
            } else {
               button.prop('disabled', false);
               msgAlert(msgAlertTypeConstant.ERROR, response.message || 'No se pudo eliminar la notificación.');
            }
         },
         error: function () {
            button.prop('disabled', false);
            msgAlert(msgAlertTypeConstant.ERROR, 'No se pudo eliminar la notificación.');
         }
      });
   });

$('#app-notification-clear-all')
   .off('click.appNotifications')
   .on('click.appNotifications', function (event) {
      event.preventDefault();
      event.stopPropagation();
      if (!this.disabled) confirmarVaciarNotificaciones();
   });

// ========================================================
// INTERACCIÓN CON NOTIFICACIONES DE CALENDARIO
// ========================================================
function abrirModuloCalendarioConEvento(eventoId, areaId) {
   if (!eventoId) return;

   var strEventoId = String(eventoId);
   var strAreaId = (areaId !== undefined && areaId !== null && String(areaId).length > 0) ? String(areaId) : null;

   // 1. Guardar en almacenamiento de sesión y estado en memoria global
   try {
      sessionStorage.setItem('erp_abrir_evento_id', strEventoId);
      if (strAreaId) {
         sessionStorage.setItem('erp_abrir_area_id', strAreaId);
      } else {
         sessionStorage.removeItem('erp_abrir_area_id');
      }
   } catch (e) { }

   window._eventoIdPendienteAbrir = strEventoId;
   window._areaIdPendienteAbrir = strAreaId;

   // 2. Cerrar dropdown de notificaciones si está desplegado
   var $dropdownToggle = $('#app-notification-trigger');
   if ($dropdownToggle.length) {
      var $parentDropdown = $dropdownToggle.closest('.dropdown');
      if ($parentDropdown.hasClass('show')) {
         $dropdownToggle.dropdown('toggle');
      }
   }

   // 3. Si ya estamos en el módulo de Calendario, abrir directamente el modal del evento
   if ($('#calendar').length > 0 && typeof window.abrirModalEventoPorId === 'function') {
      window.abrirModalEventoPorId(strEventoId, strAreaId);
      return;
   }

   // 4. Si estamos en otra vista, navegar al módulo de Calendario
   var $menuLink = $('#main-menu a.nav-link').filter(function () {
      return $(this).text().trim().toLowerCase() === 'calendario';
   });

   if ($menuLink.length > 0) {
      $menuLink.first().trigger('click');
   } else {
      if (window.updateNavbarBreadcrumb) {
         window.updateNavbarBreadcrumb(['Calendario']);
      }
      if (typeof $.fn.getView === 'function') {
         $(document.body).getView({ urlAction: 'calendario/index' });
      } else {
         window.location.href = getBaseUrl() + '/Calendario/Index';
      }
   }
}

$(document)
   .off('click.appNotificationCard', '.app-notification-card')
   .on('click.appNotificationCard', '.app-notification-card', function (event) {
      // Ignorar si se hizo clic en el botón de eliminar o sus elementos internos
      if ($(event.target).closest('.app-notification-delete, .app-notification-card-actions').length > 0) {
         return;
      }

      var card = $(this);
      var eventoId = card.attr('data-evento-id');
      var areaId = card.attr('data-area-id');

      if (eventoId) {
         abrirModuloCalendarioConEvento(eventoId, areaId);
      }
   });

// ========================================================
// CONEXIÓN SIGNALR CON REINTENTO AUTOMÁTICO
// ========================================================
var connection = new signalR.HubConnectionBuilder()
   .withUrl(getBaseUrl() + "/NotifyHub")
   .withAutomaticReconnect()
   .build();

connection.on("refreshNotifications", function (userId, data) {
   buildNotificationDropdown(JSON.parse(data));
});

connection.on("refreshNotificationList", function () {
   listarNotificaciones();
});

connection.on("calendarReminder", function (customMsg) {
   var msg = customMsg || 'Tienes un evento próximo que inicia en 15 minutos o menos. Revisa tus notificaciones.';
   msgAlert(msgAlertTypeConstant.INFORMATION, msg);
   reproducirCampanitaNotificacion();
   animarCampanitaNavbar();
   listarNotificaciones();
});

connection.onreconnected(function () {
   listarNotificaciones();
});

connection.start().then(function () {
   console.log("Hub Connected!");
   listarNotificaciones();
}).catch(function (err) {
   return console.error(err.toString());
});
