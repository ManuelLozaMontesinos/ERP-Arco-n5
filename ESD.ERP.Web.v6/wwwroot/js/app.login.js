$(document).ready(function () {
   $("#UserName").bind("keydown", function (event) {
      if (event.which === 13) {
         event.stopPropagation();
         event.preventDefault();
         $("#Password").focus();
      }
   });

   $("#Password").bind("keydown", function (event) {
      if (event.which === 13) {
         event.stopPropagation();
         event.preventDefault();
         $('#loginButton').click();
      }
   });

   $('#loginButton').click(function () {
      validateLogin();
   });

})

function validateLogin() {

   if ($("input#UserName").val() === '')
      return;

   if ($("input#Password").val() === '')
      return;

   var token = $('[name=__RequestVerificationToken]').val();

   var postData = {
      UserName: $("input#UserName").val(),
      Password: $("input#Password").val(),
      __RequestVerificationToken: token
   };

   $('#loginButton').prop('disabled', true);

   $('#loginForm').LoadingOverlay('show', {
      background: "rgba(255, 255, 255, 0.8)",
      imageResizeFactor: 0.5
   });

   var baseurl = $(location).attr('href');

   $.ajax({
      url: baseurl,
      method: 'POST',
      dataType: 'json',
      data: postData,
      error: function (xhr, ajaxOptions, thrownError) {
         $('#loginButton').prop('disabled', false);
         $('#loginForm').LoadingOverlay('hide');

         alert("Error: " + ajaxOptions + " " + thrownError);
         alert(xhr.getAllResponseHeaders());
      },
      success: function (response) {
         if (response.success) {
            window.location.href = response.url;
         }
         else {

            $('#text-login-msg').html(response.message);
            $('#div-login-msg').show();
            $('#Password').val('');
            $('#loginButton').prop('disabled', false);
            $('#loginForm').LoadingOverlay('hide');
         }
      }
   });
}