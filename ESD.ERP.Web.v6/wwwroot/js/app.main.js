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
   var offset = gridContainer.offset();
   var docHeight = $(document).height() - 20;

   var tagHeight = 50;
   var offsetTop = offset.top;

   var parent = gridContainer.parent();
   var className = parent.prop('class');
   var classNameList = className.split(' ');

   var indexClass = $.inArray("tab-pane", classNameList);
   if (indexClass >= 0) {
      if (className.indexOf('active') == -1) {

         return (parent.height() + tagHeight - 5);
         //var parentTab = parent.parent();
         //var prevElement = parentTab.prev();
         //offsetTop = prevElement.offset().top;
      }
   }

   var finalHeight = docHeight - Math.abs(offsetTop + tagHeight);

   return (finalHeight);
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
class msgAlertTypeConstant {
   static get SUCCESS() { return 1; }
   static get INFORMATION() { return 2; }
   static get ERROR() { return 3; }
};

var SwalToast = null;

function getSwalToast() {
   if (!SwalToast && typeof Swal !== 'undefined') {
      SwalToast = Swal.mixin({
         toast: true,
         position: 'top-end',
         showConfirmButton: false,
         showCloseButton: true,
         timer: 4000,
         timerProgressBar: true,
         didOpen: function (toast) {
            toast.style.cursor = 'pointer';
            toast.title = 'Click para cerrar';
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
            toast.addEventListener('click', function () {
               Swal.close();
            });
         }
      });
   }
   return SwalToast;
}

function msgAlert(type, message) {
   if (typeof Swal !== 'undefined') {
      var toastInstance = getSwalToast();
      var icon = 'info';
      switch (type) {
         case 1:
            icon = 'success';
            break;
         case 2:
            icon = 'warning';
            break;
         case 3:
            icon = 'error';
            break;
      }
      toastInstance.fire({
         icon: icon,
         title: message
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
      message: function (msg) { msgAlert(2, msg); }
   };
}
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
         var width = $('#' + gridContainer).width();
         width = width - 2; // Fudge factor to prevent horizontal scrollbars
         if (width > 0 &&
            // Only resize if new width exceeds a minimal threshold
            // Fixes IE issue with in-place resizing when mousing-over frame bars
            Math.abs(width - $(this.element).width()) > 5) {
            $(this.element).jqGrid('setGridWidth', width);
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

         this.disabledButton({ buttonId: 'new', disabled: false });

         if (isNew === true)
            return;

         var list;
         if (isEditableGrid)
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
         title: 'Â¿EstÃ¡ seguro?',
         message: '',
         onConfirm: null,
         onCancel: null
      };
      var settings = $.extend(defaults, options);

      if (typeof Swal !== 'undefined') {
         Swal.fire({
            title: settings.title || 'Â¿EstÃ¡ seguro?',
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
