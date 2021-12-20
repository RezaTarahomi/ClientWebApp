var connection = new signalR.HubConnectionBuilder().withUrl("/myownHub").withAutomaticReconnect().build();

connection.start();

document.getElementById("btnsentLetter").addEventListener("click", function () {



    //$("#btnsentLetter").on('click', function (e) {
        //e.preventDefault();

        //کنترل اینکه نامه انتخاب شده باشد
        if ($("#letterid").html().trim() == 0) {
            $("#divmsg").removeClass('hidden');
            $("#divmsg").addClass('alert-danger');
            $("#divmsg").html("هیچ نامه ای انتخاب نشده است");
            return false;
        }

        if (LetterType == 1) {
            //کنترل اینکه کاربر حتما یک شخص را جهت ارسال نامه انتخاب کرده باشد
            if ($("#SelectedUserToSent").val() == "" || $("#SelectedUserToSent").val() == '[]') {
                $("#divmsg").removeClass('hidden');
                $("#divmsg").addClass('alert-danger');
                $("#divmsg").html("هیچ شخصی جهت ارسال نامه انتخاب نشده است");
                return false;
            }
        }
        $.ajax({
            type: "Post",
            url: '/UserArea/Draft/SentLetter',
            data: {
                'LetterID': $("#letterid").html().trim(),
                'SelectedUserToSent': $("#SelectedUserToSent").val(),
                'LetterType': LetterType,
                'MainLetterID': MainLetterID
            }
        }).done(function (result) {
            if (result.status == 'noletterselected') {
                $("#divmsg").removeClass('hidden');
                $("#divmsg").addClass('alert-danger');
                $("#divmsg").html("هیچ نامه ای انتخاب نشده است");
            }
            if (result.status == 'nouserselected') {
                $("#divmsg").removeClass('hidden');
                $("#divmsg").addClass('alert-danger');
                $("#divmsg").html("هیچ شخصی جهت ارسال نامه انتخاب نشده است");
            }
            if (result.status == 'error') {
                $("#divmsg").removeClass('hidden');
                $("#divmsg").addClass('alert-danger');
                $("#divmsg").html("در ارسال نامه مشکلی به وجود آمده است");
            }
            if (result.status == 'ok') {

                var userIdList = $("#SelectedUserToSent").val();
                connection.invoke("SentLetters", userIdList).catch(function (err) {
                    return console.error(err.tostring());
                });


                swal({
                    title: "ارسال نامه",
                    text: "نامه شما با موفقیت ارسال شد",
                    type: 'success',
                    showCancelButton: false,
                    allowOutsideClick: false,
                    confirmButtonColor: "green",
                    confirmButtonText: "باشه"
                }).then(function () {
                    window.location.reload(true);
                });
            }

        });
    //});

});