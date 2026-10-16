function listarNotificaciones() {
   $(this).getAction({
      urlAction: getBaseUrl() + '/ListarNotificaciones',
      showLoading: false,
      hideLoading: false,
      onSuccess: function (response) {
         if (response.success) {

            var data = response.data;
            buildNotificationDropdown(data);

         } else {
            msgAlert(msgAlertTypeConstant.INFORMATION, response.message);
         }
      }
   });
};
function buildNotificationDropdown(data) {
   var dropdownDiv = $('#app-notification-dropdown');
   dropdownDiv.empty();

   if (data.total === 0) {
      $('#app-notification-icon').text('');
      $('#app-notification-total').text('Sin Notificaciones');

   } else {
      $('#app-notification-icon').text(data.total);
      $('#app-notification-total').text(data.total + ' Notificaciones');

      var content = '';
      var i;
      for (i = 0; i < data.list.length; ++i) {

         var item = data.list[i];
         content += "<div class='dropdown-divider'></div>" +
            "<a href='#' class='dropdown-item'>" +
            "<div class='media'>" +
            "<div class='media-body'>" +
            "<h3 class='dropdown-item-title'>" +
            item.titulo +
            "</h3>" +
            "<p class='text-sm'>" + item.descripcion + "</p>" +
            "<p class='text-sm text-muted'><i class='far fa-clock mr-1'></i> " + item.fecha + "</p>" +
            "</div>" +
            "</div>" +
            "</a>";

         if (i >= 4)
            break;
      }

      if (data.total > 5) {
         content += "<div class='dropdown-divider'></div>" +
            "<a href='#' class='dropdown-item dropdown-footer'>Todas las Notificaciones</a>"
      }

      dropdownDiv.wrapInner(content);
   }
};


var connection = new signalR.HubConnectionBuilder().withUrl(getBaseUrl() + "/NotifyHub").build();

connection.on("refreshNotifications", function (userId, data) {
   buildNotificationDropdown(JSON.parse(data));
});

connection.start().then(function () {
   console.log("Hub Connected!");
   listarNotificaciones();
}).catch(function (err) {
   return console.error(err.toString());
});

