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

   dropdown.empty();
   $('#app-notification-icon').text(total > 0 ? total : '');
   $('#app-notification-total').text(total);
   $('#app-notification-clear-all').prop('disabled', notifications.length === 0);
   $('#app-notification-empty').prop('hidden', notifications.length > 0);

   notifications.forEach(function (item) {
      var id = item.id || item.notificacionPushId;
      var title = item.titulo || 'Notificación';
      var card = $('<article>').addClass('app-notification-card').attr('role', 'listitem');
      var icon = $('<span>').addClass('app-notification-card-icon').append(
         $('<i>').addClass('far fa-bell').attr('aria-hidden', 'true')
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
      $('<time>').addClass('app-notification-card-time').text(item.fecha || '').appendTo(content);
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
