$(function () {

    var placeholderElement = $('#chatRoom-modal');
    $('button[data-toggle="ajax-chatRoom-modal"]').click(function (event) {

        $.get("/Chat/CreateGroup").done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });

    });

    placeholderElement.on('click', '[data-save="modal-cr"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');

        var selected_values = new Array();
        selected_values = []; // initialize empty array
        var list = $(".users-list option:selected");
        var listSeparated = list.map(function () {
            selected_values.push(this.value);
        });

        console.log(selected_values.length);
        selected_values.forEach(function (value, index) {
            console.log(value);
        });

        $(".users-list:checked").each(function () {
                selected_values.push($(this).val());
            });

        var actionUrl = "/Chat/CreateGroup";
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');

                toastrAlert("success", "Camera adaugata cu succes");
                window.location.reload();
            }
        });
    })
});

$("#deleteGroupButton").on('click', function () {

    swal({
        title: "Sunteti sigur?",
        text: "In urma stergerii, datele nu vor putea fi recuperate!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((sterge) => {
        if (sterge) {
            console.log(sterge);
            $.ajax({
                type: "GET",
                url: "/Chat/DeleteGroup?id=" + roomId,
                success: function (data) {
                    if (data.success) {
                        window.location.href = "/Chat/Chat/" + data.chatId;
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }, 
                error: function (e) {
                    console.log(e);
                }
            });
        }
    });
});