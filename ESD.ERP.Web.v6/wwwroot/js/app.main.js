function getBaseUrl() {
   var url = window.location.href;
   var lastIndex = url.lastIndexOf("/");
   var result = url.substring(0, lastIndex)

   return result;
};

$.isNullOrEmpty = function (s) {
   return (s === undefined || s === null || s === "");
};

$.isEmpty = function (obj) {
   var hasOwnProperty = Object.prototype.hasOwnProperty;
   // null and undefined are "empty"
   if (obj == null) return true;

   // Assume if it has a length property with a non-zero value
   // that that property is correct.
   if (obj.length > 0) return false;
   if (obj.length === 0) return true;

   // Otherwise, does it have any properties of its own?
   // Note that this doesn't handle
   // toString and valueOf enumeration bugs in IE < 9
   for (var key in obj) {
      if (hasOwnProperty.call(obj, key)) return false;
   }

   return true;
};

function sleep(milliseconds) {
   var start = new Date().getTime();
   for (var i = 0; i < 1e7; i++) {
      if ((new Date().getTime() - start) > milliseconds) {
         break;
      }
   }
}

function calculateGridHeight(container) {

   var gridContainer = $('#' + container);
   if (!gridContainer.length) {
      return 250;
   }

   var parent = gridContainer.parent();
   var className = (parent && parent.prop('class')) ? parent.prop('class') : '';
   var classNameList = className ? className.split(' ') : [];

   var indexClass = $.inArray("tab-pane", classNameList);
   if (indexClass >= 0) {
      if (className.indexOf('active') == -1) {
         var ph = parent.height();
         return (ph > 0 ? ph : 250);
      }
   }

   var offset = gridContainer.offset();
   var docHeight = $(window).height();
   var tagHeight = 110;
   var offsetTop = offset ? offset.top : 0;

   var finalHeight = docHeight - Math.abs(offsetTop + tagHeight);

   if (finalHeight < 150) {
      finalHeight = 150;
   }

   return Math.round(finalHeight);
};

var ViewModelState = {
   getValue: function (prop) {
      var obj = $.parseJSON($('#__vms').val().replace(/\'/g, '"'));
      return obj[prop];
   },
   setValue: function (prop, value) {

      var obj = $.parseJSON($('#__vms').val().replace(/\'/g, '"'));
      obj[prop] = value;

      var str = JSON.stringify(obj);
      $('#__vms').val(str);
   },
   create: function (prop, value) {
      var obj = {};
      obj[prop] = value;

      return obj;
   }
};

$.fn.addItems = function (data) {
   return this.each(function () {
      var list = this;
      $.each(data, function (index, item) {
         var option = new Option(item.text, item.value);
         list.add(option);

         if (item.Selected)
            $('#' + list.id).val(item.Value);
      });
   });
};

function setModifiedDTO(dto) {

   if (dto.DTOState === "0")
      return;

   dto.DTOState = "1";
};

function parseIntEx(value) {
   return parseInt(value.toString().replace(/,/g, ''));
};
function parseFloatEx(value) {
   return parseFloat(value.toString().replace(/,/g, ''));
};
function roundEx(value, decimals) {
   return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}
function padLeadingZeros(num, size) {
   var s = num + "";
   while (s.length < size) s = "0" + s;
   return s;
};
/*
 * --------------------------------------------
 * validate.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.validateString = function (message) {
      return this.validateControl({ element: this, message: message });
   };
   $.fn.validateNumeric = function (message) {
      return this.validateControl({ element: this, message: message, type: 'numeric' });
   };
   $.fn.validateControl = function (options) {
      var defaults = {
         message: '',
         type: ''
      };
      var settings = $.extend(defaults, options);

      if ($(this).length === 0) {
         msgAlert(msgAlertTypeConstant.ERROR, 'El elemento <' + $(this).selector + '> no existe');
         return false;
      }

      var type = 'string';
      var isValid = true;

      if (!$.isNullOrEmpty(settings.type))
         type = settings.type;

      if ($.isNullOrEmpty($(this).val() || '')) {
         isValid = false;
      } else {
         switch (type) {
            case 'numeric':
               var valueEval = parseFloat($(this).val());
               if (valueEval === 0) {
                  isValid = false;
               }
               break;
         }
      }

      if (!isValid && !$.isNullOrEmpty(settings.message)) {
         msgAlert(msgAlertTypeConstant.INFORMATION, settings.message);
         $(this).focus();
      }

      return isValid;
   };
})(jQuery);
/*
 * --------------------------------------------
 * getView.js
 * --------------------------------------------
 */
(function ($) {
   window.aplicarPermisosVista = function (c, settings, jqXHR) {
   if (!c || c.length === 0) {
      c = $('#workspace');
   }
   settings = settings || {};

   var headerSoloLectura = jqXHR && typeof jqXHR.getResponseHeader === 'function' ? jqXHR.getResponseHeader('X-App-SoloLectura') : null;
   var isSoloLectura = false;
   if (headerSoloLectura !== null && headerSoloLectura !== undefined && headerSoloLectura !== '') {
      isSoloLectura = (headerSoloLectura === 'true');
      window.esSoloLectura = isSoloLectura;
   } else if (typeof window.esSoloLectura === 'boolean') {
      isSoloLectura = window.esSoloLectura;
   } else {
      isSoloLectura = false;
      window.esSoloLectura = false;
   }
   $('#__esSoloLectura').val(isSoloLectura ? 'true' : 'false');

   var hasBandejaMarkers = c.find('#bandejaGrid, #bandejaContainer, #app-search-card').length > 0;
   var isExplicitDetail = (settings.editAction === 'Edit' || settings.editAction === 'New');
   var isBandejaView = hasBandejaMarkers && !isExplicitDetail;

   if (isSoloLectura) {
      if (c.find('#calendar, #modal-evento-calendario, #external-events').length > 0) {
         return;
      }

      if (isBandejaView) {
         if (window.removeBreadcrumbSubAction) { window.removeBreadcrumbSubAction(); }

         // In Bandeja: Hide write actions from main menubar
         c.find('[id$="_opt_new"], [id$="_opt_delete"], [id$="_opt_save"], [id$="_opt_cancel"], [id$="_opt_soli_auto"], [id$="_opt_gen_pedido"], [id$="_opt_gen_compro"], [id$="_opt_cambiar_cotizacion"], [id$="_opt_cambiar_estado"], [id$="_opt_enviar_prod"], [id$="_opt_cerrar"], [id$="_opt_extornar"]').hide();

         // Change 'Editar' to 'Consultar' with search icon
         c.find('#menuBar_opt_edit, [id$="_opt_edit"]').each(function() {
            var btn = $(this);
            btn.find('span').attr('class', 'fas fa-search');
            var textNode = btn.contents().filter(function() { return this.nodeType === 3; });
            if (textNode.length > 0) {
               textNode[0].nodeValue = ' Consultar';
            } else {
               btn.html('<span class="fas fa-search"></span> Consultar');
            }
         });

         // In Bandeja / Index / Search views: Filters, search fields and buttons must remain 100% active and operative!
         c.find('#app-search-card input, #app-search-card select, #app-search-card textarea, #app-search-input-group input, #app-search-input-group select, #app-search-input-group textarea').prop('disabled', false);
         c.find('#app-search-card button, #app-search-card .input-group-append, #app-search-card .input-group-append *, #app-search-card .app-button-textbox button, #app-search-card .btn, #clearButton, [id$="clearButton"], [id$="searchButton"]').prop('disabled', false).show();
      } else {
         // Detail / Form / Consultation view
         if (window.appendNavbarBreadcrumb) { window.appendNavbarBreadcrumb('Consultar'); }

         // Hide write buttons in main menubar
         c.find('#menuBar [id$="_opt_new"], #menuBar [id$="_opt_delete"], #menuBar [id$="_opt_save"], #menuBar [id$="_opt_cancel"], #menuBar [id$="_opt_soli_auto"], #menuBar [id$="_opt_gen_pedido"], #menuBar [id$="_opt_gen_compro"], #menuBar [id$="_opt_cambiar_cotizacion"], #menuBar [id$="_opt_cambiar_estado"], #menuBar [id$="_opt_enviar_prod"], #menuBar [id$="_opt_cerrar"], #menuBar [id$="_opt_extornar"]').hide();

         // Hide secondary menubars (detail grids like productoMenuBar, entregaMenuBar)
         c.find('.esd-menubar:not(#menuBar)').hide();

         var bloquearCamposLectura = function () {
            var target = c.find('#app-content').length > 0 ? c.find('#app-content') : c;

            // Disable all textboxes, selects, textareas outside search card
            target.find('input:not([type=hidden]), select, textarea')
               .not('#app-search-card *, #app-search-input-group *')
               .prop('disabled', true);

            // Disable checkboxes and radios and prevent clicking on labels/wrappers
            target.find('input[type=checkbox], input[type=radio]')
               .not('#app-search-card *, #app-search-input-group *')
               .prop('disabled', true);
            target.find('.icheck-primary, .custom-control, .custom-checkbox, .custom-radio')
               .not('#app-search-card *, #app-search-input-group *')
               .css('pointer-events', 'none');

            // Disable Select2 dropdowns
            target.find('select.select2, select[data-select2-id]').prop('disabled', true);
            target.find('.select2-container')
               .not('#app-search-card *, #app-search-input-group *')
               .css('pointer-events', 'none')
               .addClass('select2-container--disabled');

            // Disable and lock DateTimePicker triggers (calendar icons)
            target.find('[data-toggle="datetimepicker"], [data-target*="datetimepicker"], .input-group-append:has(.fa-calendar), .input-group-append:has(.fa-calendar-alt), .input-group-append:has([data-toggle="datetimepicker"])')
               .not('#app-search-card *, #app-search-input-group *')
               .prop('disabled', true)
               .css('pointer-events', 'none');

            // Disable and hide custom button-textbox / search popup buttons
            target.find('.app-button-textbox button, .app-input-group-button, .input-group-append button')
               .not('#app-search-card *, #app-search-input-group *')
               .prop('disabled', true)
               .css('pointer-events', 'none')
               .hide();

            // Disable action buttons inside form (assign, directions, etc.)
            target.find('button.btn')
               .not('#clearButton, [id$="clearButton"], [id$="searchButton"], #menuBar *, #app-search-card *, #app-search-input-group *')
               .prop('disabled', true)
               .css('pointer-events', 'none');

            // Disable detail jqgrid inputs and interactions
            target.find('.ui-jqgrid-bdiv input, .ui-jqgrid-bdiv select, .ui-jqgrid-bdiv textarea, .ui-jqgrid-bdiv button')
               .prop('disabled', true);
         };

         bloquearCamposLectura();
         setTimeout(bloquearCamposLectura, 150);
      }
   } else {
      // WRITE MODE (Escritura):
      window.esSoloLectura = false;

      if (isBandejaView) {
         if (window.removeBreadcrumbSubAction) { window.removeBreadcrumbSubAction(); }

         // Ensure write buttons in menubar are shown
         c.find('[id$="_opt_new"], [id$="_opt_delete"], [id$="_opt_save"], [id$="_opt_cancel"], [id$="_opt_soli_auto"], [id$="_opt_gen_pedido"], [id$="_opt_gen_compro"], [id$="_opt_cambiar_cotizacion"], [id$="_opt_cambiar_estado"], [id$="_opt_enviar_prod"], [id$="_opt_cerrar"], [id$="_opt_extornar"]').show();

         // Restore 'Editar' text if previously changed to 'Consultar'
         c.find('#menuBar_opt_edit, [id$="_opt_edit"]').each(function() {
            var btn = $(this);
            var textNode = btn.contents().filter(function() { return this.nodeType === 3; });
            if (textNode.length > 0 && textNode[0].nodeValue.trim() === 'Consultar') {
               textNode[0].nodeValue = ' Editar';
               btn.find('span').attr('class', 'fas fa-pencil-alt');
            }
         });
      } else {
         // Form / Detail Edit or Create view in Write Mode
         if (settings.editAction === 'Edit' && window.appendNavbarBreadcrumb) {
            window.appendNavbarBreadcrumb('Editar');
         } else if (settings.editAction === 'New' && window.appendNavbarBreadcrumb) {
            window.appendNavbarBreadcrumb('Nuevo');
         }

         // Show secondary menubars (e.g. productoMenuBar, entregaMenuBar)
         c.find('.esd-menubar').show();

         // Ensure pointer-events are unblocked
         c.find('[data-toggle="datetimepicker"], [data-target*="datetimepicker"], .input-group-append, .select2-container, .icheck-primary, .custom-control').css('pointer-events', '');

         // Ensure lookup buttons are visible
         c.find('.input-group-append button, .app-button-textbox button').show();
      }
   }
};

   $.fn.getView = function (options) {
      var defaults = {
         rowId: 0,
         editAction: '',
         menuBarId: 'menuBar',
         buttonId: '',
         urlAction: '',
         showLoading: true,
         hideLoading: true,
         container: 'workspace',
         onSuccess: null,
         type: 'POST',
         contentType: "application/json; charset=utf-8",
         dataType: 'html',
         viewModel: null,
         selModel: null
      };
      var settings = $.extend(defaults, options);

      var elementContainer;
      if (!$(this).is('div')) {
         elementContainer = $('#workspace');
      }
      else {
         if ($(this).length > 0) {
            var selector = $(this)[0].id;
            var modalContent = $('#' + selector).find('.modal-content');
            if (modalContent.length > 0) {
               elementContainer = modalContent;
            } else
               elementContainer = $('#workspace');
         }
      }

      var data;

      this.each(function () {

         if (!$.isNullOrEmpty(settings.menuBarId)) {
            if (!$.isNullOrEmpty(settings.buttonId))
               $("#" + settings.menuBarId).appMenu('disabledButton', { buttonId: settings.buttonId, disabled: true });
         }

         if (settings.viewModel !== null) {

            var model = modelToObject(settings.viewModel);

            if (settings.selModel !== null) {
               var selModel = createSelectedItemList(settings.selModel);
               model['SelectedItemList'] = selModel;
            }

            data = model;
         } else {
            if (settings.selModel !== null) {
               var selModel = createSelectedItemList(settings.selModel);
               data = {
                  SelectedItemList: selModel,
                  ViewModelState: ViewModelState.create('CID', $('#__cid').val())
               };
            }
         }

         $.ajax({
            beforeSend: function () {
               if (settings.showLoading === true)
                  elementContainer.showLoadingOverlay();
            },
            complete: function () {
               if (settings.hideLoading === true)
                  elementContainer.hideLoadingOverlay();
            },
            type: settings.type,
            contentType: settings.contentType,
            url: settings.urlAction,
            headers: { 'x-app-cid': $('#__cid').val() || '' },
            data: JSON.stringify(data),
            dataType: settings.dataType,
            success: function (response, textStatus, jqXHR) {
               if (!$.isNullOrEmpty(settings.container)) {
                  $("#" + settings.container).empty();
                  $("#" + settings.container).html(response);
               if (settings.urlAction && window.menuUrlMap) {
                  var targetUrl = settings.urlAction.trim().toLowerCase().split('?')[0];
                  var noSlash = targetUrl.replace(/^\/+/, '');
                  var foundPath = window.menuUrlMap[targetUrl] || window.menuUrlMap[noSlash] || window.menuUrlMap['/' + noSlash];
                  if (foundPath && window.updateNavbarBreadcrumb) {
                     window.updateNavbarBreadcrumb(foundPath);
                  }
               }
               }

               var c = !$.isNullOrEmpty(settings.container) ? $("#" + settings.container) : $('#workspace');
               if (typeof window.aplicarPermisosVista === 'function') {
                  window.aplicarPermisosVista(c, settings, jqXHR);
               }

               if (typeof settings.onSuccess === 'function')
                  settings.onSuccess(response);

                if (settings.container === 'workspace') {
                   window.requestAnimationFrame(function () {
                      var workspace = document.getElementById('workspace');
                      if (workspace) workspace.scrollTop = 0;
                      if (document.scrollingElement) document.scrollingElement.scrollTop = 0;
                      window.scrollTo(0, 0);
                   });
                }
            },
            error: function (xhr, status, error) {

               if (xhr.status === "999") {

                  var url;
                  var message;
                  if (this.dataType === "html") {
                     var response = JSON.parse(xhr.responseText);
                     url = response.Url;
                     message = response.Message;
                  }
                  else {
                     url = xhr.responseJSON.Url;
                     message = xhr.responseJSON.Message;
                  }

                  if (typeof Swal !== 'undefined') {
                     Swal.fire({
                        title: 'Aviso del Sistema',
                        html: message,
                        icon: 'warning',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        allowOutsideClick: false
                     });
                  } else if (typeof bootbox !== 'undefined') {
                     bootbox.dialog({
                        message: '<p class="text-center" style="color: #cc3f44;font-size: 13px;font-weight: bold;padding: 8px 0px 0px 0px;">' + message + '</p>',
                        closeButton: false
                     });
                  }

                  setTimeout(function () {
                     window.location.href = url;
                  }, 2000);
                  return;
               }

               if ($.isNullOrEmpty(error.toString()))
                  error = xhr.statusText;

               if (xhr.readyState === 0)
                  error = "Se perdio la conexion con el Servidor. Verifique su conexion a Internet.";

               msgAlert(msgAlertTypeConstant.ERROR, error.toString());
            }
         });

      });
   };
})(jQuery);
function modelToObject(viewModel) {
   var model = {};
   var i = 0;
   var value = null;

   if ($('#__vms').length > 0) {
      var vms = $('#__vms').val().replace(/'/g, '"');
      model['ViewModelState'] = $.parseJSON(vms);
      model['ViewModelState'].CID = $('#__cid').val();
   } else {
      model['ViewModelState'] = ViewModelState.create('CID', $('#__cid').val());
   }

   buildObjectModel(viewModel, model);

   return model;
};
function buildObjectModel(viewModel, model) {

   for (var i = 0, count = viewModel.length; i < count; i++) {
      var item = viewModel[i];

      if (item.type === 'list') {
         model[item.name] = [];
         for (var j = 0; j < item.value.length; j++) {
            var listModel = {};
            buildObjectModel(item.value[j], listModel);
            model[item.name].push(listModel);
         }
      }
      else {
         value = getValueFromModel(item);
         stringToObj(item.name, value, model);
      }
   };
};
function getValueFromModel(item) {

   var value = item.value;

   var dataType = item.type;
   if ($.isNullOrEmpty(dataType))
      dataType = '';

   var textCase = item.textCase;
   if ($.isNullOrEmpty(textCase))
      textCase = 'upper';

   var castNull = item.castNull;
   if ($.isNullOrEmpty(castNull))
      castNull = false;

   switch (dataType) {
      case 'date':
         if ($.isNullOrEmpty(value))
            value = null;
         else {
            value = moment(value, 'DD/MM/YYYY').toISOString(true);
         }
         break;
      case 'datetime':
         if ($.isNullOrEmpty(value))
            value = null;
         else {
            value = moment(value, 'DD/MM/YYYY hh:mm:ss').toISOString(true);
         }
         break;

      case 'int':
      case 'decimal':
         if ($.isNullOrEmpty(value)) {
            if (!castNull)
               value = null;
            else
               value = JSON.parse((dataType == 'int' ? '0' : '0.00'));
         }
         else
            value = JSON.parse(parseFloatEx(value));
         break;

      case 'listInt':
         value = _.map(value, function (i) { return parseIntEx(i); });
         break;

      case 'listString':
         value = _.map(value, function (i) { return i; });
         break;

      case 'bool':
         break;

      default:
         if ($.isNullOrEmpty(value))
            value = '';

         switch (textCase) {
            case 'lower':
               value = value.toLowerCase();
               break;
            case 'upper':
               value = value.toUpperCase();
               break;
         }
         break;
   }

   return value;
};
function stringToObj(path, value, obj) {
   var objValue = value;
   //try {
   //   objValue = JSON.parse(value);
   //} catch (e) { } //eat the error, must not be json so carry on... Hack to do a valid JSON check

   var parts = path.split("."), part;
   var last = parts.pop();
   while (part = parts.shift()) {
      if (typeof obj[part] != "object")
         obj[part] = {};
      obj = obj[part];
   }
   if (obj.hasOwnProperty(last) && obj[last] && obj[last].constructor === Array) {
      obj[last].push(objValue);
   }
   else if (obj.hasOwnProperty(last) && obj[last]) {
      var objArray = [];
      objArray.push(obj[last])
      objArray.push(objValue);
      obj[last] = objArray;
   }
   else {
      obj[last] = objValue;
   }
};
function createSelectedItemList(selModel) {
   var selItemList = [];

   for (var i = 0, count = selModel.length; i < count; i++) {
      var item = selModel[i];

      var name = item.name;
      if ($.isNullOrEmpty(name))
         continue;

      var type = item.type;
      if ($.isNullOrEmpty(type))
         type = 'value';

      var disabled = item.disabled;
      if ($.isNullOrEmpty(disabled))
         disabled = false;

      var value = item.value;
      if ($.isNullOrEmpty(value))
         value = '';

      var sel = {
         Name: name,
         Disabled: disabled,
         SelectedItemType: type,
         Value: value
      };

      selItemList.push(sel);
   };

   if (selItemList.length === 0)
      return null;

   return selItemList;
}
/*
 * --------------------------------------------
 * getAction.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.getAction = function (options) {
      var defaults = {
         urlAction: '',
         showLoading: false,
         hideLoading: false,
         contentType: "application/json; charset=utf-8",
         dataType: 'json',
         id: null,
         data: null,
         onSuccess: null
      };
      var settings = $.extend(defaults, options);

      var elementContainer;
      if (!$(this).is('div')) {
         elementContainer = $('#workspace');
      }
      else {
         if ($(this).length > 0) {
            var selector = $(this)[0].id;
            var modalContent = $('#' + selector).find('.modal-content');
            if (modalContent.length > 0) {
               elementContainer = modalContent;
            }
         }
      }

      this.each(function () {
         $.ajax({
            beforeSend: function () {
               if (settings.showLoading === true) {
                  if (!$.isNullOrEmpty(elementContainer))
                     elementContainer.showLoadingOverlay();
                  else
                     $("#ajaxLoadingPanel").show();
               }
            },
            complete: function () {
               if (settings.hideLoading === true) {
                  if (!$.isNullOrEmpty(elementContainer))
                     elementContainer.hideLoadingOverlay();
                  else
                     setTimeout(function () {
                        $("#ajaxLoadingPanel").hide();
                     }, 200);
               }
            },
            type: 'GET',
            contentType: settings.contentType,
            url: settings.urlAction,
            headers: { 'x-app-cid': $('#__cid').val() || '' },
            data: (settings.id !== null ? { id: settings.id } : settings.data),
            dataType: settings.dataType,
            success: function (response, textStatus, jqXHR) {
               if (typeof settings.onSuccess === 'function')
                  settings.onSuccess(response);
            },
            error: function (xhr, status, error) {

               if (xhr.status === "999") {

                  var url;
                  var message;
                  if (this.dataType === "html") {
                     var response = JSON.parse(xhr.responseText);
                     url = response.Url;
                     message = response.Message;
                  }
                  else {
                     url = xhr.responseJSON.Url;
                     message = xhr.responseJSON.Message;
                  }

                  if (typeof Swal !== 'undefined') {
                     Swal.fire({
                        title: 'Aviso del Sistema',
                        html: message,
                        icon: 'warning',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        allowOutsideClick: false
                     });
                  } else if (typeof bootbox !== 'undefined') {
                     bootbox.dialog({
                        message: '<p class="text-center" style="color: #cc3f44;font-size: 13px;font-weight: bold;padding: 8px 0px 0px 0px;">' + message + '</p>',
                        closeButton: false
                     });
                  }

                  setTimeout(function () {
                     window.location.href = url;
                  }, 2000);
                  return;
               }

               if ($.isNullOrEmpty(error.toString()))
                  error = xhr.statusText;

               if (xhr.readyState === 0)
                  error = "Se perdio la conexion con el Servidor. Verifique su conexion a Internet.";

               msgAlert(msgAlertTypeConstant.ERROR, error.toString());
            }
         });
      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * postAction.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.postAction = function (options) {
      var defaults = {
         urlAction: '',
         showLoading: true,
         hideLoading: true,
         onSuccess: null,
         type: 'POST',
         contentType: "application/json; charset=utf-8",
         dataType: 'json',
         viewModel: null
      };
      var settings = $.extend(defaults, options);

      var elementContainer;
      if (!$(this).is('div')) {
         elementContainer = $('#workspace');
      }
      else {
         if ($(this).length > 0) {
            var selector = $(this)[0].id;
            var modalContent = $('#' + selector).find('.modal-content');
            if (modalContent.length > 0) {
               elementContainer = modalContent;
            }
         }
      }

      var data;
      this.each(function () {

         if (settings.viewModel !== null) {
            var item = modelToObject(settings.viewModel);

            if ($('#__vms').length > 0) {
               var vms = $('#__vms').val().replace(/'/g, '"');
               item['ViewModelState'] = $.parseJSON(vms);
               item['ViewModelState'].CID = $('#__cid').val();
            }

            data = item;
         }

         $.ajax({
            beforeSend: function () {
               if (settings.showLoading === true)
                  elementContainer.showLoadingOverlay();
            },
            complete: function () {
               if (settings.hideLoading === true)
                  elementContainer.hideLoadingOverlay();
            },
            type: settings.type,
            contentType: settings.contentType,
            url: settings.urlAction,
            data: JSON.stringify(data),
            dataType: settings.dataType,
            success: function (response, textStatus, jqXHR) {
               if (typeof settings.onSuccess === 'function')
                  settings.onSuccess(response);
            },
            error: function (xhr, status, error) {

               if (xhr.status === "999") {

                  var url;
                  var message;
                  if (this.dataType === "html") {
                     var response = JSON.parse(xhr.responseText);
                     url = response.Url;
                     message = response.Message;
                  }
                  else {
                     url = xhr.responseJSON.Url;
                     message = xhr.responseJSON.Message;
                  }

                  if (typeof Swal !== 'undefined') {
                     Swal.fire({
                        title: 'Aviso del Sistema',
                        html: message,
                        icon: 'warning',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        allowOutsideClick: false
                     });
                  } else if (typeof bootbox !== 'undefined') {
                     bootbox.dialog({
                        message: '<p class="text-center" style="color: #cc3f44;font-size: 13px;font-weight: bold;padding: 8px 0px 0px 0px;">' + message + '</p>',
                        closeButton: false
                     });
                  }

                  setTimeout(function () {
                     window.location.href = url;
                  }, 2000);
                  return;
               }

               if ($.isNullOrEmpty(error.toString()))
                  error = xhr.statusText;

               if (xhr.readyState === 0)
                  error = "Se perdio la conexion con el Servidor. Verifique su conexion a Internet.";

               msgAlert(msgAlertTypeConstant.ERROR, error.toString());
            }
         });

      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * getReport.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.getReport = function (options) {
      var defaults = {
         menuBarId: 'menuBar',
         buttonId: '',
         urlAction: '',
         parameters: '',
         format: 'PDF',
         onFailMessage: ''
      };
      var settings = $.extend(defaults, options);

      var realURL = settings.urlAction + '?format=' + settings.format;

      if (!$.isNullOrEmpty(settings.parameters))
         realURL = realURL + '&' + settings.parameters;

      realURL = realURL + "&cid=" + $('#__cid').val();

      this.each(function () {

         if (!$.isNullOrEmpty(settings.menuBarId))
            $("#" + settings.menuBarId).appMenu('disabledButton', { buttonId: settings.buttonId, disabled: true });

         $.fileDownload(realURL, {
            successCallback: function (url) {
               if (!$.isNullOrEmpty(settings.menuBarId))
                  $("#" + settings.menuBarId).appMenu('disabledButton', { buttonId: settings.buttonId, disabled: false });

            },
            failCallback: function (html, url) {
               msgAlert(msgAlertTypeConstant.INFORMATION, settings.onFailMessage);

               if (!$.isNullOrEmpty(settings.menuBarId))
                  $("#" + settings.menuBarId).appMenu('disabledButton', { buttonId: settings.buttonId, disabled: false });
            }
         });
      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * saveView.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.saveView = function (options) {
      var defaults = {
         editAction: '',
         menuBarId: 'menuBar',
         buttonId: '',
         urlAction: '',
         showLoading: true,
         hideLoading: true,
         container: '',
         onSuccess: null,
         type: 'POST',
         contentType: "application/json; charset=utf-8",
         dataType: 'json',
         viewModel: null
      };
      var settings = $.extend(defaults, options);

      var elementContainer;
      if (!$(this).is('div')) {
         elementContainer = $('#workspace');
      }
      else {
         if ($(this).length > 0) {
            var selector = $(this)[0].id;
            var modalContent = $('#' + selector).find('.modal-content');
            if (modalContent.length > 0) {
               elementContainer = modalContent;
            } else {
               elementContainer = $('#' + selector);
            }
         }
      }

      var data;
      this.each(function () {

         if (!$.isNullOrEmpty(settings.menuBarId))
            $("#" + settings.menuBarId).appMenu('disabledButton', { buttonId: settings.buttonId, disabled: true });

         if (settings.viewModel !== null) {
            var item = modelToObject(settings.viewModel);

            if ($('#__vms').length > 0) {
               var vms = $('#__vms').val().replace(/'/g, '"');
               item['ViewModelState'] = $.parseJSON(vms);
               item['ViewModelState'].CID = $('#__cid').val();
            }

            data = item;
         }

         $.ajax({
            beforeSend: function () {
               if (settings.showLoading === true)
                  elementContainer.showLoadingOverlay();
            },
            complete: function () {
               if (settings.hideLoading === true)
                  elementContainer.hideLoadingOverlay();
            },
            type: settings.type,
            contentType: settings.contentType,
            url: settings.urlAction,
            data: JSON.stringify(data),
            dataType: settings.dataType,
            success: function (response, textStatus, jqXHR) {
               if (!$.isNullOrEmpty(settings.container)) {
                  $("#" + settings.container).empty();
                  $("#" + settings.container).html(response);
               if (settings.urlAction && window.menuUrlMap) {
                  var targetUrl = settings.urlAction.trim().toLowerCase().split('?')[0];
                  var noSlash = targetUrl.replace(/^\/+/, '');
                  var foundPath = window.menuUrlMap[targetUrl] || window.menuUrlMap[noSlash] || window.menuUrlMap['/' + noSlash];
                  if (foundPath && window.updateNavbarBreadcrumb) {
                     window.updateNavbarBreadcrumb(foundPath);
                  }
               }
               }

               var c = !$.isNullOrEmpty(settings.container) ? $("#" + settings.container) : $('#workspace');
               if (typeof window.aplicarPermisosVista === 'function') {
                  window.aplicarPermisosVista(c, settings, jqXHR);
               }

               if (typeof settings.onSuccess === 'function')
                  settings.onSuccess(response);
            },
            error: function (xhr, status, error) {

               if (xhr.status === "999") {

                  var url;
                  var message;
                  if (this.dataType === "html") {
                     var response = JSON.parse(xhr.responseText);
                     url = response.Url;
                     message = response.Message;
                  }
                  else {
                     url = xhr.responseJSON.Url;
                     message = xhr.responseJSON.Message;
                  }

                  if (typeof Swal !== 'undefined') {
                     Swal.fire({
                        title: 'Aviso del Sistema',
                        html: message,
                        icon: 'warning',
                        showConfirmButton: false,
                        timer: 2000,
                        timerProgressBar: true,
                        allowOutsideClick: false
                     });
                  } else if (typeof bootbox !== 'undefined') {
                     bootbox.dialog({
                        message: '<p class="text-center" style="color: #cc3f44;font-size: 13px;font-weight: bold;padding: 8px 0px 0px 0px;">' + message + '</p>',
                        closeButton: false
                     });
                  }

                  setTimeout(function () {
                     window.location.href = url;
                  }, 2000);
                  return;
               }

               if ($.isNullOrEmpty(error.toString()))
                  error = xhr.statusText;

               if (xhr.readyState === 0)
                  error = "Se perdio la conexion con el Servidor. Verifique su conexion a Internet.";

               msgAlert(msgAlertTypeConstant.ERROR, error.toString());
            }
         });

      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * LoadingOverlay.js
 * --------------------------------------------
*/
(function ($) {
   $.fn.showLoadingOverlay = function () {
      var container = $(this);
      var isDark = $('body').hasClass('dark-mode');
      this.each(function () {
         container.LoadingOverlay('show', {
            background: isDark ? "rgba(33, 37, 41, 0.85)" : "rgba(255, 255, 255, 0.8)",
            imageColor: isDark ? "#38bdf8" : undefined,
            imageResizeFactor: 0.5
         });
      });
   };
})(jQuery);
(function ($) {
   $.fn.hideLoadingOverlay = function () {
      var container = $(this);
      this.each(function () {
         container.LoadingOverlay('hide');
      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * msgAlert.js (SweetAlert2 Modern Toast System)
 * --------------------------------------------
*/
var msgAlertTypeConstant = window.msgAlertTypeConstant || class {
   static get SUCCESS() { return 1; }
   static get INFORMATION() { return 2; }
   static get ERROR() { return 3; }
};

var SwalToast = null;
var SwalToastOutsideClickHandler = null;
var SwalToastOutsideClickElement = null;

function clearSwalToastOutsideClickHandler() {
   if (SwalToastOutsideClickHandler && typeof document !== 'undefined') {
      document.removeEventListener('pointerdown', SwalToastOutsideClickHandler, true);
   }
   SwalToastOutsideClickHandler = null;
   SwalToastOutsideClickElement = null;
}

function bindSwalToastOutsideClick(toast) {
   clearSwalToastOutsideClickHandler();
   SwalToastOutsideClickElement = toast;
   SwalToastOutsideClickHandler = function (event) {
      if (!toast || !toast.isConnected) {
         clearSwalToastOutsideClickHandler();
         return;
      }

      if (toast.contains(event.target)) return;

      clearSwalToastOutsideClickHandler();
      if (SwalToast && typeof SwalToast.close === 'function') {
         SwalToast.close();
      }
   };
   document.addEventListener('pointerdown', SwalToastOutsideClickHandler, true);
}

function getSwalToast() {
   if (!SwalToast && typeof Swal !== 'undefined') {
      SwalToast = Swal.mixin({
         toast: true,
         position: 'top-end',
         showConfirmButton: false,
         showCloseButton: true,
         timer: 3000,
         timerProgressBar: true,
         customClass: {
            container: 'app-toast-container',
            popup: 'app-toast'
         },
         willClose: function (toast) {
            if (toast === SwalToastOutsideClickElement) {
               clearSwalToastOutsideClickHandler();
            }
         }
      });
   }
   return SwalToast;
}

function msgAlert(type, message) {
   if (typeof Swal !== 'undefined') {
      var toastInstance = getSwalToast();
      var icon = 'info';
      var iconHtml = '<i class="far fa-bell" aria-hidden="true"></i>';
      switch (type) {
         case 1:
            icon = 'success';
            iconHtml = '<i class="fas fa-check" aria-hidden="true"></i>';
            break;
         case 2:
            icon = 'warning';
            iconHtml = '<i class="fas fa-exclamation-triangle" aria-hidden="true"></i>';
            break;
         case 3:
            icon = 'error';
            iconHtml = '<i class="fas fa-times" aria-hidden="true"></i>';
            break;
      }
      toastInstance.fire({
         icon: icon,
         iconHtml: iconHtml,
         title: message,
         didOpen: function (toast) {
            toast.classList.add('app-toast--' + icon);
            bindSwalToastOutsideClick(toast);
         }
      });
      return;
   }

   if (typeof alertify !== 'undefined') {
      alertify.set('notifier', 'delay', 4);
      alertify.set('notifier', 'position', 'top-right');
      switch (type) {
         case 1:
            alertify.success(message);
            break;
         case 2:
            alertify.warning(message);
            break;
         case 3:
            alertify.error(message);
            break;
      }
   }
}

if (typeof window !== 'undefined') {
   window.alertify = {
      set: function () { },
      success: function (msg) { msgAlert(1, msg); },
      warning: function (msg) { msgAlert(2, msg); },
      error: function (msg) { msgAlert(3, msg); },
      message: function (msg) { msgAlert(1, msg); }
   };
}
/*
 * --------------------------------------------
 * Auto-ordenamiento inicial por Código (jqGrid)
 * Aplica a todos los módulos y sus detalles (bandejas, popups y grids hijas)
 * --------------------------------------------
*/
(function ($) {
   if (!$.fn || !$.fn.jqGrid) return;

   var _origJqGrid = $.fn.jqGrid;

   function detectarColumnaCodigo(options) {
      if (!options || !Array.isArray(options.colModel)) return null;

      var colModel = options.colModel;
      var colNames = Array.isArray(options.colNames) ? options.colNames : [];

      // 1. Prioridad: Columna visible cuyo 'name' sea exactamente "Codigo" (case-insensitive)
      for (var i = 0; i < colModel.length; i++) {
         var col = colModel[i];
         if (col && col.name && !col.hidden) {
            var colNameLower = col.name.trim().toLowerCase();
            if (colNameLower === 'codigo') {
               return { name: col.name, index: i };
            }
         }
      }

      // 2. Prioridad: Columna visible cuyo encabezado en 'colNames' sea "Código" o "Codigo"
      for (var j = 0; j < colNames.length; j++) {
         var label = (colNames[j] || '').toString().trim().toLowerCase();
         if (/^c[oó\u00f3\ufffd]digo$/i.test(label) || label === 'codigo' || label === 'código') {
            if (j < colModel.length && colModel[j] && colModel[j].name && !colModel[j].hidden) {
               return { name: colModel[j].name, index: j };
            }
         }
      }

      // 3. Prioridad: Columna visible cuyo 'name' empiece por "Codigo" específico (ej. CodigoProducto, CodigoReferencial)
      for (var k = 0; k < colModel.length; k++) {
         var colK = colModel[k];
         if (colK && colK.name && !colK.hidden) {
            var nLower = colK.name.trim().toLowerCase();
            if (nLower === 'codigoproducto' || nLower === 'codigoreferencial' || nLower === 'codigoarticulo') {
               return { name: colK.name, index: k };
            }
         }
      }

      return null;
   }

   function obtenerValorFila(row, colName, colIndex) {
      if (!row) return '';
      if (row.cell !== undefined) {
         if (Array.isArray(row.cell) && colIndex !== undefined && colIndex >= 0 && colIndex < row.cell.length) {
            return row.cell[colIndex];
         }
         if (typeof row.cell === 'object' && row.cell !== null) {
            if (row.cell[colName] !== undefined) return row.cell[colName];
            for (var k in row.cell) {
               if (k.toLowerCase() === colName.toLowerCase()) return row.cell[k];
            }
         }
         return row.cell;
      }
      if (row[colName] !== undefined) return row[colName];
      for (var prop in row) {
         if (prop.toLowerCase() === colName.toLowerCase()) return row[prop];
      }
      return '';
   }

   function resolverValorCelda(cellVal, rowData, colName, colIndex) {
      if (cellVal !== undefined && cellVal !== null && cellVal !== '') {
         return cellVal;
      }
      if (rowData && typeof rowData === 'object') {
         if (rowData.cell !== undefined) {
            if (Array.isArray(rowData.cell) && colIndex !== undefined && colIndex >= 0 && colIndex < rowData.cell.length) {
               var cv = rowData.cell[colIndex];
               if (cv !== undefined && cv !== null && cv !== '') return cv;
            } else if (typeof rowData.cell === 'object' && rowData.cell !== null) {
               if (rowData.cell[colName] !== undefined && rowData.cell[colName] !== null && rowData.cell[colName] !== '') {
                  return rowData.cell[colName];
               }
               var colLowerCell = (colName || '').toLowerCase();
               for (var kc in rowData.cell) {
                  if (kc.toLowerCase() === colLowerCell) {
                     var vc = rowData.cell[kc];
                     if (vc !== undefined && vc !== null && vc !== '') return vc;
                  }
               }
            }
         }
         if (rowData[colName] !== undefined && rowData[colName] !== null && rowData[colName] !== '') {
            return rowData[colName];
         }
         var colLower = (colName || '').toLowerCase();
         for (var k in rowData) {
            if (k.toLowerCase() === colLower) {
               var v = rowData[k];
               if (v !== undefined && v !== null && v !== '') return v;
            }
         }
      }
      return cellVal;
   }

   function detectarColumnaFecha(options) {
      if (!options || !Array.isArray(options.colModel)) return null;
      var colModel = options.colModel;
      var colNames = Array.isArray(options.colNames) ? options.colNames : [];

      for (var i = 0; i < colModel.length; i++) {
         var col = colModel[i];
         if (col && col.name && !col.hidden) {
            if (col.name.trim().toLowerCase() === 'fecha') {
               return { name: col.name, index: i };
            }
         }
      }

      for (var j = 0; j < colModel.length; j++) {
         var colJ = colModel[j];
         if (colJ && colJ.name && !colJ.hidden) {
            if (colJ.sorttype === 'date' || colJ.datefmt) {
               return { name: colJ.name, index: j };
            }
         }
      }

      for (var k = 0; k < colNames.length; k++) {
         var label = (colNames[k] || '').toString().trim().toLowerCase();
         if (label === 'fecha' || label.indexOf('fecha') !== -1) {
            if (k < colModel.length && colModel[k] && colModel[k].name && !colModel[k].hidden) {
               return { name: colModel[k].name, index: k };
            }
         }
      }

      return null;
   }

   function detectarModuloOrdenFecha(options) {
      if (!options) return false;
      if (options.sortname && /^fecha/i.test(options.sortname.trim())) {
         return true;
      }
      var url = (options.url || '').toLowerCase();
      var pathname = (window.location && window.location.pathname ? window.location.pathname : '').toLowerCase();
      var checkStr = url + ' ' + pathname;

      var modulosFecha = [
         'merchandising',
         'canjeselectivo',
         'cajacobranza',
         'cobranza',
         'comprobante',
         'notacredito',
         'guiaremi',
         'guiaremvta',
         'guiaremotro'
      ];

      for (var i = 0; i < modulosFecha.length; i++) {
         if (checkStr.indexOf(modulosFecha[i]) !== -1) {
            return true;
         }
      }
      return false;
   }

   function parsearFechaUniversal(val) {
      if (val === null || val === undefined || val === '') return null;
      if (typeof val === 'number') return (!isNaN(val) && isFinite(val)) ? val : null;
      if (val instanceof Date) return isNaN(val.getTime()) ? null : val.getTime();
      var s = String(val).trim();
      if (s.indexOf('<') !== -1 && s.indexOf('>') !== -1) {
         s = s.replace(/<[^>]*>/g, '').trim();
      }
      s = s.replace(/&(?:nbsp|#160);/gi, '').trim();
      if (!s) return null;

      // Timestamp numérico como string (10 a 13 dígitos)
      if (/^\d{10,13}$/.test(s)) {
         var numTs = parseInt(s, 10);
         if (!isNaN(numTs)) return numTs;
      }

      // /Date(1234567890)/
      var dateMatch = s.match(/\/Date\((\d+)\)\//);
      if (dateMatch) return parseInt(dateMatch[1], 10);

      // dd/MM/yyyy o dd/MM/yyyy HH:mm:ss o dd-MM-yyyy
      var dmy = s.match(/^(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})(?:\s+(\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?/);
      if (dmy) {
         var d = parseInt(dmy[1], 10);
         var m = parseInt(dmy[2], 10) - 1;
         var y = parseInt(dmy[3], 10);
         var hh = dmy[4] ? parseInt(dmy[4], 10) : 0;
         var mm = dmy[5] ? parseInt(dmy[5], 10) : 0;
         var ss = dmy[6] ? parseInt(dmy[6], 10) : 0;
         var dt = new Date(y, m, d, hh, mm, ss);
         return isNaN(dt.getTime()) ? null : dt.getTime();
      }

      // yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss o yyyy/MM/dd
      var ymd = s.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})(?:[T\s](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?/);
      if (ymd) {
         var y2 = parseInt(ymd[1], 10);
         var m2 = parseInt(ymd[2], 10) - 1;
         var d2 = parseInt(ymd[3], 10);
         var hh2 = ymd[4] ? parseInt(ymd[4], 10) : 0;
         var mm2 = ymd[5] ? parseInt(ymd[5], 10) : 0;
         var ss2 = ymd[6] ? parseInt(ymd[6], 10) : 0;
         var dt2 = new Date(y2, m2, d2, hh2, mm2, ss2);
         return isNaN(dt2.getTime()) ? null : dt2.getTime();
      }

      var dtFallback = new Date(s);
      return isNaN(dtFallback.getTime()) ? null : dtFallback.getTime();
   }

   function parsearNumeroUniversal(val) {
      if (val === null || val === undefined || val === '') return null;
      if (typeof val === 'number') {
         if (isNaN(val) || !isFinite(val)) return null;
         return val;
      }
      var s = String(val);
      if (s.indexOf('<') !== -1 && s.indexOf('>') !== -1) {
         s = s.replace(/<[^>]*>/g, '');
      }
      s = s.replace(/&(?:nbsp|#160);/gi, '');
      s = s.replace(/S\/\.?|\$|\€/gi, '').replace(/[^\d\.\,\-]/g, '').trim();
      if (!s || s === '-') return null;
      if (s.indexOf(',') !== -1 && s.indexOf('.') !== -1) {
         if (s.lastIndexOf('.') > s.lastIndexOf(',')) {
            s = s.replace(/,/g, '');
         } else {
            s = s.replace(/\./g, '').replace(',', '.');
         }
      } else if (s.indexOf(',') !== -1 && s.indexOf('.') === -1) {
         if (/,\d{1,2}$/.test(s)) {
            s = s.replace(',', '.');
         } else {
            s = s.replace(/,/g, '');
         }
      }
      var num = parseFloat(s);
      return (isNaN(num) || !isFinite(num)) ? null : num;
   }

   function limpiarTextoParaComparar(val) {
      if (val === null || val === undefined) return '';
      if (typeof val === 'object') {
         if (val.Nombre !== undefined) return limpiarTextoParaComparar(val.Nombre);
         if (val.Codigo !== undefined) return limpiarTextoParaComparar(val.Codigo);
         if (val.name !== undefined) return limpiarTextoParaComparar(val.name);
         return '';
      }
      var s = String(val);
      if (s.indexOf('<') !== -1 && s.indexOf('>') !== -1) {
         s = s.replace(/<[^>]*>/g, '');
      }
      s = s.replace(/&(?:nbsp|#160);/gi, ' ');
      s = s.trim();
      s = s.replace(/^["'«»“”‘’\s\(\)\[\]\-]+/, '').replace(/["'«»“”‘’\s\(\)\[\]\-]+$/, '');
      return s.trim();
   }

   function compararTextoNatural(a, b, direction) {
      var dir = (direction === -1 || direction === 'desc' || direction === 'd') ? -1 : 1;
      var sA = (typeof a === 'string') ? a : limpiarTextoParaComparar(a);
      var sB = (typeof b === 'string') ? b : limpiarTextoParaComparar(b);
      if (!sA && !sB) return 0;
      if (!sA) return 1;
      if (!sB) return -1;
      return dir * sA.localeCompare(sB, 'es', { numeric: true, sensitivity: 'base' });
   }

   function configurarOrdenamientoInteligente(options) {
      if (!options || typeof options !== 'object' || !Array.isArray(options.colModel)) {
         return;
      }

      var colModel = options.colModel;
      var esModuloFecha = detectarModuloOrdenFecha(options);
      var infoFecha = esModuloFecha ? detectarColumnaFecha(options) : null;
      var infoCodigo = (!infoFecha) ? detectarColumnaCodigo(options) : null;

      // 1. Establecer sortname y sortorder iniciales
      if (infoFecha) {
         options.sortname = infoFecha.name;
         if (!options.sortorder || options.sortorder === '') {
            options.sortorder = 'desc';
         }
      } else if (infoCodigo) {
         if (!options.sortname || options.sortname === '') {
            options.sortname = infoCodigo.name;
            if (!options.sortorder || options.sortorder === '') {
               options.sortorder = 'asc';
            }
         }
      }

      // 2. Optimizar cada columna en colModel para que el ordenamiento sea consistente y preciso
      for (var i = 0; i < colModel.length; i++) {
         var col = colModel[i];
         if (!col || typeof col !== 'object') continue;

         var colName = (col.name || '').trim();

         // Asegurar que index coincida con name para que getAccessor no retorne undefined
         if (!col.index || col.index === '') {
            col.index = colName;
         }

         // Columnas vacías o sin nombre no deben permitir click de ordenamiento
         if (!colName || colName === '') {
            col.sortable = false;
            continue;
         }

         // Si el desarrollador ya proveyó un sortfunc propio, respetarlo
         if (col.sortfunc && typeof col.sortfunc === 'function') {
            continue;
         }

         var esColumnaCodigo = infoCodigo && (col.name === infoCodigo.name || i === infoCodigo.index);
         var colNameLower = colName.toLowerCase();
         var esFecha = col.sorttype === 'date' || !!col.datefmt || col.formatter === 'date' || /fecha|date/i.test(colNameLower);
         var esNumero = col.sorttype === 'number' || col.sorttype === 'integer' || col.sorttype === 'float' ||
                        col.formatter === 'number' || col.formatter === 'currency' || col.formatter === 'integer' ||
                        /(monto|total|precio|cantidad|saldo|importe|subtotal|igv|neto|tasa|porcentaje|descuento|valorunitario|costo|pesoneto|pesobruto|volumen)/i.test(colNameLower);
         var esDocumentoOCodigo = esColumnaCodigo || /^(codigo|#?documento|numerodocumento|ruc|serie|correlativo|numero|nro|guia|orden|cotizac|pedido)/i.test(colNameLower);

         if (esFecha) {
            col.sorttype = (function (cName, cIdx) {
               return function (cellVal, rowData) {
                  var val = resolverValorCelda(cellVal, rowData, cName, cIdx);
                  return parsearFechaUniversal(val);
               };
            })(colName, i);
            col.sortfunc = function (a, b, direction) {
               var dir = (direction === -1 || direction === 'desc' || direction === 'd') ? -1 : 1;
               var tA = (typeof a === 'number' && !isNaN(a)) ? a : parsearFechaUniversal(a);
               var tB = (typeof b === 'number' && !isNaN(b)) ? b : parsearFechaUniversal(b);
               if (tA === null && tB === null) return 0;
               if (tA === null) return 1;
               if (tB === null) return -1;
               if (tA === tB) return 0;
               return tA < tB ? -dir : dir;
            };
         } else if (esNumero) {
            col.sorttype = (function (cName, cIdx) {
               return function (cellVal, rowData) {
                  var val = resolverValorCelda(cellVal, rowData, cName, cIdx);
                  return parsearNumeroUniversal(val);
               };
            })(colName, i);
            col.sortfunc = function (a, b, direction) {
               var dir = (direction === -1 || direction === 'desc' || direction === 'd') ? -1 : 1;
               var nA = (typeof a === 'number' && !isNaN(a)) ? a : parsearNumeroUniversal(a);
               var nB = (typeof b === 'number' && !isNaN(b)) ? b : parsearNumeroUniversal(b);
               if (nA === null && nB === null) return 0;
               if (nA === null) return 1;
               if (nB === null) return -1;
               if (nA === nB) return 0;
               return nA < nB ? -dir : dir;
            };
         } else if (esDocumentoOCodigo) {
            col.sorttype = (function (cName, cIdx) {
               return function (cellVal, rowData) {
                  var val = resolverValorCelda(cellVal, rowData, cName, cIdx);
                  return limpiarTextoParaComparar(val);
               };
            })(colName, i);
            col.sortfunc = function (a, b, direction) {
               return compararTextoNatural(a, b, direction);
            };
         } else {
            col.sorttype = (function (cName, cIdx) {
               return function (cellVal, rowData) {
                  var val = resolverValorCelda(cellVal, rowData, cName, cIdx);
                  return limpiarTextoParaComparar(val);
               };
            })(colName, i);
            col.sortfunc = function (a, b, direction) {
               return compararTextoNatural(a, b, direction);
            };
         }
      }

      // 3. Pre-ordenar datos locales al inicializar (ej. grids de subdetalles o arrays en memoria)
      if (infoFecha && Array.isArray(options.data) && options.data.length > 1) {
         var targetNameLocalF = infoFecha.name;
         var targetIndexLocalF = infoFecha.index;
         var isDescLocal = ((options.sortorder || '').toLowerCase() !== 'asc');
         options.data.sort(function (itemA, itemB) {
            var vA = resolverValorCelda('', itemA, targetNameLocalF, targetIndexLocalF);
            var vB = resolverValorCelda('', itemB, targetNameLocalF, targetIndexLocalF);
            var tA = parsearFechaUniversal(vA);
            var tB = parsearFechaUniversal(vB);
            if (tA === null && tB === null) return 0;
            if (tA === null) return 1;
            if (tB === null) return -1;
            if (tA === tB) return 0;
            return tA < tB ? (isDescLocal ? 1 : -1) : (isDescLocal ? -1 : 1);
         });
      } else if (infoCodigo && Array.isArray(options.data) && options.data.length > 1) {
         var targetNameLocal = infoCodigo.name;
         var targetIndexLocal = infoCodigo.index;
         options.data.sort(function (itemA, itemB) {
            var vA = obtenerValorFila(itemA, targetNameLocal, targetIndexLocal);
            var vB = obtenerValorFila(itemB, targetNameLocal, targetIndexLocal);
            return compararTextoNatural(vA, vB);
         });
      }

      // 4. Interceptar onSortCol para registrar que el usuario hizo click en una columna
      var origOnSortCol = options.onSortCol;
      options.onSortCol = function (index, iCol, sortorder) {
         if (this && this.p) {
            this.p._userSorted = true;
         }
         if (typeof origOnSortCol === 'function') {
            return origOnSortCol.apply(this, arguments);
         }
      };

      // 5. Interceptar la respuesta JSON para ordenar inicialmente por Fecha (desc) o Código (asc) SIN sobreescribir clicks del usuario
      if (infoFecha || infoCodigo) {
         var originalBeforeProcessing = options.beforeProcessing;
         options.beforeProcessing = function (data) {
            if (data) {
               var currentSort = (this && this.p && this.p.sortname) ? this.p.sortname : (options.sortname || '');
               var currentOrder = (this && this.p && this.p.sortorder) ? this.p.sortorder : (options.sortorder || '');
               var userHasSorted = this && this.p && this.p._userSorted;

               var rowsArray = null;
               if (Array.isArray(data)) {
                  rowsArray = data;
               } else if (Array.isArray(data.rows)) {
                  rowsArray = data.rows;
               }

               if (rowsArray && rowsArray.length > 1) {
                  if (infoFecha && (!userHasSorted) && (currentSort === '' || currentSort === infoFecha.name)) {
                     var targetNameF = infoFecha.name;
                     var targetIndexF = infoFecha.index;
                     var isDesc = (currentOrder.toLowerCase() !== 'asc');
                     rowsArray.sort(function (itemA, itemB) {
                        var vA = resolverValorCelda('', itemA, targetNameF, targetIndexF);
                        var vB = resolverValorCelda('', itemB, targetNameF, targetIndexF);
                        var tA = parsearFechaUniversal(vA);
                        var tB = parsearFechaUniversal(vB);
                        if (tA === null && tB === null) return 0;
                        if (tA === null) return 1;
                        if (tB === null) return -1;
                        if (tA === tB) return 0;
                        return tA < tB ? (isDesc ? 1 : -1) : (isDesc ? -1 : 1);
                     });
                  } else if (infoCodigo && (!userHasSorted) && (currentSort === '' || currentSort === infoCodigo.name)) {
                     var targetNameC = infoCodigo.name;
                     var targetIndexC = infoCodigo.index;
                     rowsArray.sort(function (itemA, itemB) {
                        var vA = obtenerValorFila(itemA, targetNameC, targetIndexC);
                        var vB = obtenerValorFila(itemB, targetNameC, targetIndexC);
                        return compararTextoNatural(vA, vB);
                     });
                  }
               }
            }
            if (typeof originalBeforeProcessing === 'function') {
               return originalBeforeProcessing.apply(this, arguments);
            }
         };
      }
   }

   

   // Usar Proxy para envolver $.fn.jqGrid preservando el 100% de métodos y propiedades internas (getStyleUI, getGridParam, etc.)
   var customJqGrid;
   if (typeof Proxy === 'function') {
      customJqGrid = new Proxy(_origJqGrid, {
         apply: function (target, thisArg, argArray) {
            if (argArray && argArray.length > 0 && typeof argArray[0] === 'object' && argArray[0] !== null) {
               configurarOrdenamientoInteligente(argArray[0]);
            }
            return Reflect.apply(target, thisArg, argArray);
         }
      });
   } else {
      customJqGrid = function (options) {
         if (options && typeof options === 'object') {
            configurarOrdenamientoInteligente(options);
         }
         return _origJqGrid.apply(this, arguments);
      };
      $.extend(customJqGrid, _origJqGrid);
   }

   $.fn.jqGrid = customJqGrid;
})(jQuery);

/*
 * --------------------------------------------
 * appGrid.js
 * --------------------------------------------
*/
(function ($) {

   var pluginName = 'appGrid';
   var defaults;

   function Plugin(element, options) {
      this.element = element;
      this.options = $.extend({}, defaults, options);
      this._name = pluginName;
      this.init();
   }

   Plugin.prototype = {
      init: function (name) {
         //this.hello();
      },
      rowCount: function () {
         var records = $(this.element).jqGrid('getGridParam', 'records');

         return records;
      },
      getData: function () {
         var data = $(this.element).jqGrid('getGridParam', 'data');

         return data;
      },
      getSelectedRowId: function () {
         var rowid = $(this.element).jqGrid('getGridParam', 'selrow');

         return rowid;
      },
      getSelectedRowIdList: function () {
         var rows = $(this.element).jqGrid('getGridParam', 'selarrrow');

         return rows;
      },
      getSelectedRow: function () {
         var rowid = this.getSelectedRowId();
         if (rowid === null)
            return [];

         return this.getRow(rowid);
      },
      getRow: function (rowid) {
         var row = $(this.element).jqGrid('getRowData', rowid);
         return row;
      },
      getRowData: function () {
         var row = $(this.element).jqGrid('getRowData');
         return row;
      },
      getLocalRow: function (rowid) {
         var row = $(this.element).jqGrid('getLocalRow', rowid);
         return row;
      },
      editableRowsCount: function () {
         var length = $(this.element).find("[editable='1']").length;
         return length;
      },
      isEditableRow: function (rowid) {
         if ($($(this.element).jqGrid("getInd", rowid, true)).attr("editable") != '1') {
            return false;
         };
         return true;
      },
      reloadJSON: function () {
         $(this.element).jqGrid('setGridParam', { datatype: 'json' });
         $(this.element).trigger('reloadGrid');

         return true;
      },
      getDataIDs: function () {
         var list = $(this.element).jqGrid('getDataIDs');

         return list;
      },
      addRow: function (initdata) {
         var records = $(this.element).jqGrid('getGridParam', 'records');
         var parameters = {
            rowID: records + 1,
            initdata: initdata,
            position: "afterSelected"
         };
         $(this.element).jqGrid('addRow', parameters);

         return true;
      },
      resizeGrid: function (gridContainer) {
         if (!gridContainer) return true;
         var $container = $('#' + gridContainer);
         if (!$container.length) return true;

         var width = $container.width();
         width = width - 2; // Fudge factor to prevent horizontal scrollbars
         if (width > 0 &&
            // Only resize if new width exceeds a minimal threshold
            // Fixes IE issue with in-place resizing when mousing-over frame bars
            Math.abs(width - $(this.element).width()) > 5) {
            $(this.element).jqGrid('setGridWidth', width);
         }

         var newH = calculateGridHeight(gridContainer);
         if (newH >= 150) {
            var curH = parseFloat($(this.element).jqGrid('getGridParam', 'height'));
            if (isNaN(curH) || Math.abs(newH - curH) > 5) {
               $(this.element).jqGrid('setGridHeight', newH);
            }
         }

         return true;
      }
   };

   $.fn[pluginName] = function (options) {

      var retval = null;

      if (typeof options === "string") {
         var args = Array.prototype.slice.call(arguments, 1);

         this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName))
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));

            var plugin = $.data(this, 'plugin_' + pluginName);
            retval = plugin[options].apply(plugin, args);
         });
      }
      else {
         return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));
            }
         });
      }

      if (retval === null) {
         retval = this;
      }

      return retval;
   };
})(jQuery);
function isEmptyRecord(rowId) {
   if (rowId === 'norecs')
      return true;

   return false;
}
/*
 * --------------------------------------------
 * appMenu.js
 * --------------------------------------------
*/
(function ($) {

   var pluginName = 'appMenu';
   var defaults;

   function Plugin(element, options) {
      this.element = element;
      this.options = $.extend({}, defaults, options);
      this._name = pluginName;
      this.init();
   }

   Plugin.prototype = {
      init: function (name) {
         var self = this;
      },
      disabled: function () {
         var i = 0;
         var buttonList = $(this.element).find(":button");
         var buttonPrefix = '_opt_';

         for (i = 0; i < buttonList.length; ++i) {
            this.disabledButton({ buttonId: buttonList[i].id.replace($(this.element)[0].id + buttonPrefix, ''), disabled: true });
         }
      },
      disabledButton: function (options) {
         var localDefaults = {
            buttonId: '',
            buttonList: '',
            disabled: false
         };

         var id = $(this.element)[0].id;
         var localSettings = $.extend(localDefaults, options);

         if ($.isNullOrEmpty(localSettings.buttonList))
            localSettings.buttonList = localSettings.buttonId

         var buttonList = localSettings.buttonList.split(',');
         var buttonPrefix = '_opt_';
         for (i = 0; i < buttonList.length; ++i) {
            var ctl = $("#" + id + buttonPrefix + buttonList[i]);
            ctl.attr("disabled", localSettings.disabled);
         }
      },
      hideButton: function (options) {
         var localDefaults = {
            buttonId: '',
            buttonList: ''
         };

         var id = $(this.element)[0].id;

         var localSettings = $.extend(localDefaults, options);
         if (!$.isNullOrEmpty(localSettings.buttonId)) {

            var buttonPrefix = '_opt_';

            var ctl = $("#" + id + buttonPrefix + localSettings.buttonId);
            ctl.hide();
         }
      },
      showButton: function (options) {
         var localDefaults = {
            buttonId: '',
            buttonList: ''
         };

         var id = $(this.element)[0].id;

         var localSettings = $.extend(localDefaults, options);
         if (!$.isNullOrEmpty(localSettings.buttonId)) {

            var buttonPrefix = '_opt_';
            var ctl = $("#" + id + buttonPrefix + localSettings.buttonId);
            ctl.show();
         }
      },
      editMenu: function (options) {
         var defaults = {
            gridName: '',
            isEditableGrid: false,
            excludeDeletedRow: false
         };

         var settings = $.extend(defaults, options);
         var isNew = true;
         if (!$.isNullOrEmpty(settings.gridName)) {
            var records = $('#' + settings.gridName).appGrid('rowCount');
            isNew = records === 0 ? true : false;
            if (isNew === false) {
               var selectedRow = $('#' + settings.gridName).appGrid('getSelectedRow');
               if ($.isEmpty(selectedRow))
                  isNew = true;
               else {
                  if (!settings.excludeDeletedRow)
                     if (selectedRow.hasOwnProperty('Estado')) {
                        if (selectedRow.Estado !== 'A')
                           isNew = true;
                     }
               }
            }
         }
         this.privateEditGridMenuBar(isNew, settings.isEditableGrid);
      },
      saveMenu: function (options) {
         this.disabled();

         var list = 'save,cancel';
         this.disabledButton({ buttonList: list, disabled: false });
      },
      privateEditGridMenuBar: function (isNew, isEditableGrid) {
         this.disabled();

         var isSoloLectura = window.esSoloLectura === true;

         if (!isSoloLectura) {
            this.disabledButton({ buttonId: 'new', disabled: false });
         }

         if (isNew === true)
            return;

         var list;
         if (isSoloLectura) {
            list = 'edit,preview,exportxls,dataxls,consulta,reporte_anticipo';
         } else if (isEditableGrid)
            list = 'copy,edit,delete,preview,exportxls,dataxls';
         else
            list = 'copy,edit,delete,save,cancel,exit,preview,exportxls,dataxls';

         this.disabledButton({ buttonList: list, disabled: false });
      },
      setTitle: function (options) {
         var defaults = {
            buttonId: '',
            title: ''
         };
         var settings = $.extend(defaults, options);
         if (!$.isNullOrEmpty(settings.buttonId)) {
            var barName = $(this.element)[0].id;
            var span = $('#' + barName + '_' + settings.buttonId + ' span');
            span.next().text(settings.title);
         }
      },
      showButton: function (options) {
         var localDefaults = {
            buttonId: '',
            buttonList: ''
         };

         var id = $(this.element)[0].id;
         var localSettings = $.extend(localDefaults, options);

         if ($.isNullOrEmpty(localSettings.buttonList))
            localSettings.buttonList = localSettings.buttonId

         var buttonList = localSettings.buttonList.split(',');
         var buttonPrefix = '_opt_';
         for (i = 0; i < buttonList.length; ++i) {
            var ctl = $("#" + id + buttonPrefix + buttonList[i]);
            ctl.show();
         }
      },
      hideButton: function (options) {
         var localDefaults = {
            buttonId: '',
            buttonList: ''
         };

         var id = $(this.element)[0].id;
         var localSettings = $.extend(localDefaults, options);

         if ($.isNullOrEmpty(localSettings.buttonList))
            localSettings.buttonList = localSettings.buttonId

         var buttonList = localSettings.buttonList.split(',');
         var buttonPrefix = '_opt_';
         for (i = 0; i < buttonList.length; ++i) {
            var ctl = $("#" + id + buttonPrefix + buttonList[i]);
            ctl.hide();
         }
      }
   };

   $.fn[pluginName] = function (options) {
      if (typeof options === "string") {
         var args = Array.prototype.slice.call(arguments, 1);

         this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName))
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));

            var plugin = $.data(this, 'plugin_' + pluginName);
            return plugin[options].apply(plugin, args);
         });
      }
      else {
         return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));
            }
         });
      }
   };
})(jQuery);
/*
 * --------------------------------------------
 * confirm.js (SweetAlert2 Modern Confirm System)
 * --------------------------------------------
*/
(function ($) {
   $.fn.confirm = function (options) {
      var defaults = {
         title: '¿Está seguro?',
         message: '',
         onConfirm: null,
         onCancel: null
      };
      var settings = $.extend(defaults, options);

      if (typeof Swal !== 'undefined') {
         Swal.fire({
            title: settings.title || '¿Está seguro?',
            html: settings.message,
            icon: 'question',
            showCancelButton: true,
            reverseButtons: true,
            confirmButtonText: '<i class=\"fas fa-check mr-1\"></i> Aceptar',
            cancelButtonText: '<i class=\"fas fa-times mr-1\"></i> Cancelar',
            focusCancel: false,
            customClass: {
               popup: 'swal2-modern-modal',
               confirmButton: 'btn btn-primary px-3 py-2 mr-2',
               cancelButton: 'btn btn-secondary px-3 py-2'
            },
            buttonsStyling: false
         }).then(function (result) {
            if (result.isConfirmed) {
               if (typeof settings.onConfirm === 'function')
                  settings.onConfirm();
            } else if (result.dismiss === Swal.DismissReason.cancel) {
               if (typeof settings.onCancel === 'function')
                  settings.onCancel();
            }
         });
         return;
      }

      this.each(function () {
         if (typeof bootbox !== 'undefined') {
            bootbox.confirm({
               title: settings.title,
               message: settings.message,
               buttons: {
                  confirm: {
                     label: 'Aceptar',
                     className: 'btn-primary btn-sm'
                  },
                  cancel: {
                     label: 'Cancelar',
                     className: 'btn-secondary btn-sm'
                  }
               },
               callback: function (result) {
                  if (result === true) {
                     if (typeof settings.onConfirm === 'function')
                        settings.onConfirm();
                  } else {
                     if (typeof settings.onCancel === 'function')
                        settings.onCancel();
                  }
               }
            });
         }
      });
   };
})(jQuery);
/*
 * --------------------------------------------
 * dialog.js
 * --------------------------------------------
*/
(function ($) {
   $.fn.setDialog = function (options) {
      var selector = $(this)[0].id;
      var defaults = {
         onShow: null,
         onShown: null,
         onHidden: null
      };
      var settings = $.extend(defaults, options);
      this.each(function () {
         $("#" + selector).on('show.bs.modal', function () {
            $(this).find('input:text').val('');

            var params = $("#" + selector).data('dialog-params');
            if (typeof settings.onShow === 'function')
               settings.onShow(params);
         });

         $("#" + selector).on('shown.bs.modal', function () {
            if (typeof settings.onShown === 'function')
               settings.onShown();
         });

         $("#" + selector).on('hidden.bs.modal', function (e) {
            $('#' + selector + 'Content').empty();

            if (typeof settings.onHidden === 'function')
               settings.onHidden();
         });
      });
   };
})(jQuery);
(function ($) {
   $.fn.showDialog = function (options) {

      var defaults = {
         editAction: '',
         hideColumnList: ''
      };
      var settings = $.extend(defaults, options);

      var selector = $(this)[0].id;
      settings.dialogId = '';

      this.each(function () {

         if ($("#" + selector + 'Content').length > 0) {
            $("#" + selector + 'Content').data('dialog-id', selector);
            settings.dialogId = selector;
         }

         $("#" + selector).data('dialog-params', settings)
         $("#" + selector).modal('show');
      });
   };
})(jQuery);
(function ($) {
   $.fn.hideDialog = function () {
      var selector = $(this)[0].id;
      this.each(function () {
         $("#" + selector).modal('hide');
         //$(document.body).removeClass('modal-open');
         $('.modal-backdrop').remove();
      });
   };
})(jQuery);
function getPopupParam(elem) {
   var container = $('#' + elem).parent();
   var dialogId = container.data('dialog-id');
   if (dialogId != null) {
      var params = $("#" + dialogId).data('dialog-params');

      return params;
   }

   return {
      editAction: '',
      dialogId: '',
      hideColumnList: ''
   };
}
/*
 * --------------------------------------------
 * appButtonTextBox.js
 * --------------------------------------------
*/
(function ($) {

   var pluginName = 'appButtonTextBox';
   var defaults;

   function Plugin(element, options) {
      this.element = element;
      this.options = $.extend({}, defaults, options);
      this._name = pluginName;
      this.init();
   }

   Plugin.prototype = {
      init: function (name) {
         //this.hello();
      },
      disabled: function (options) {
         var defaults = {
            buttonId: '',
            buttonList: '',
            disabled: true
         };

         var settings = $.extend(defaults, options);

         var id = $(this.element)[0].id;
         var buttonList = [];

         if ($.isNullOrEmpty(settings.buttonId) && $.isNullOrEmpty(settings.buttonList)) {
            var container = $(this.element).parent();

            container.find(":button").each(function () {
               buttonList.push($(this).attr("id"));
            });
         }
         else {
            if ($.isNullOrEmpty(settings.buttonList))
               settings.buttonList = settings.buttonId;

            var tmpArr = settings.buttonList.split(',');
            for (var i = 0; i < tmpArr.length; i++) {
               buttonList.push(id + '_button_' + tmpArr[i])
            }
         }

         $.each(buttonList, function (index, ctl) {
            $('#' + ctl).attr("disabled", settings.disabled);
         });

         return true;
      },
      allowEdit: function (state) {

         var id = $(this.element)[0].id;
         $('#' + id).prop("readonly", !state);

         return true;
      }
   };

   $.fn[pluginName] = function (options) {

      var retval = null;

      if (typeof options === "string") {
         var args = Array.prototype.slice.call(arguments, 1);

         this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName))
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));

            var plugin = $.data(this, 'plugin_' + pluginName);
            retval = plugin[options].apply(plugin, args);
         });
      }
      else {
         return this.each(function () {
            if (!$.data(this, 'plugin_' + pluginName)) {
               $.data(this, 'plugin_' + pluginName, new Plugin(this, options));
            }
         });
      }

      if (retval === null) {
         retval = this;
      }

      return retval;
   };
})(jQuery);
/*
 * --------------------------------------------
 * uploapFile.js
 * --------------------------------------------
 */
(function ($) {
   $.fn.uploadFile = function (options) {
      var defaults = {
         browseButton: '',
         archivoExcel: '',
         container: 'container',
         urlAction: '',
         chunkSize: '3mb',
         parameters: null,
         title: null,
         extensions: null,
         onInit: '',
         onFilesAdded: '',
         onCompleted: ''
      };
      var settings = $.extend(defaults, options);

      this.each(function () {

         var uploader = new plupload.Uploader({
            runtimes: 'html5',
            browse_button: settings.browseButton,
            container: document.getElementById(settings.container),
            url: settings.urlAction,
            silverlight_xap_url: 'Scripts/Moxie.xap',
            chunk_size: settings.chunkSize,
            multiple_queues: false,
            multipart_params: settings.parameters,
            filters: [{ title: settings.title, extensions: settings.extensions }],
            headers: { 'x-app-cid': $('#__cid').val() || '' },
            init: {
               PostInit: function () {

                  if (typeof settings.onInit === 'function') {
                     settings.onInit(uploader);
                  }
               },
               BeforeUpload: function () {
                  uploader.settings.multipart_params = settings.parameters;
               },
               FilesAdded: function (up, files) {
                  if (typeof settings.onFilesAdded === 'function')
                     settings.onFilesAdded(files);
               },

               UploadProgress: function (up, file) {
                  $('.progress-bar').css('width', file.percent + '%').attr('aria-valuenow', file.percent);
               },

               FileUploaded: function (up, file, info) {
                  if (typeof settings.onCompleted === 'function')
                     settings.onCompleted(info);
               },

               Error: function (up, err) {
                  document.getElementById('console').innerHTML += "\nError #" + err.code + ": " + err.message;
               }
            }
         });

         uploader.init();
      });
   };
})(jQuery);

// Auto-redimensionamiento reactivo en cambio de tamano de ventana y tabs
(function ($) {
   var _gridResizeTimer = null;
   $(window).on('resize', function () {
      clearTimeout(_gridResizeTimer);
      _gridResizeTimer = setTimeout(function () {
         $('div[id*="Container"]').each(function () {
            var containerId = this.id;
            var grid = $(this).find('table.ui-jqgrid-btable');
            if (grid.length > 0 && grid.is(':visible')) {
               grid.appGrid('resizeGrid', containerId);
            }
         });
      }, 150);
   });

   $(document).on('shown.bs.tab', 'a[data-toggle="tab"], button[data-toggle="tab"]', function () {
      setTimeout(function () {
         $('div[id*="Container"]:visible').each(function () {
            var containerId = this.id;
            var grid = $(this).find('table.ui-jqgrid-btable');
            if (grid.length > 0) {
               grid.appGrid('resizeGrid', containerId);
            }
         });
      }, 50);
   });
})(jQuery);
