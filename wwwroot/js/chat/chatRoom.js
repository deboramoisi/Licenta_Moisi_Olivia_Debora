var urlAddUsersToGroup = "/Chat/AddUsersToGroupModal?id=";
var urlGroupUsers = "/Chat/GetGroupUsers?id=";
var urlDeleteUserFromGroup = "/Chat/DeleteGroupUser?id=";
var urlCreateGroup = "/Chat/CreateGroup";

$(function () {

    var placeholderElement = $('#chatRoom-modal');
    $('button[data-toggle="ajax-chatRoom-modal"]').click(function (event) {

        $.get(urlCreateGroup).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });

    });

    placeholderElement.on('click', '[data-save="modal-cr"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');

        var dataToSend = form.serialize();

        $.post(urlCreateGroup, dataToSend).done(function (data) {
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

    $('button[data-toggle="ajax-addUserGroup-modal"]').click(function (event) {

        $.ajax({
            type: "GET",
            url: urlAddUsersToGroup + roomId,
            success: function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            },
            error: function (e) {
                console.log(e);
            }
        });

    });

    placeholderElement.on('click', '[data-save="modal-usersGroup"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var dataToSend = form.serialize();

        $.ajax({
            type: "POST",
            url: urlAddUsersToGroup + roomId,
            data: dataToSend,
            success: function () {
                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success", "Utilizatori adaugati cu succes!");
                setTimeout(function () {
                    location.reload(true);
                }, 5000);
            }, 
            error: function (e) {
                console.log(e);
            }
        });
    })

    $('button[data-toggle="ajax-groupUsers-modal"]').click(function (event) {

        $.ajax({
            type: "GET",
            url: urlGroupUsers + roomId,
            success: function (data) {
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
            },
            error: function (e) {
                console.log(e);
            }
        });

    }); 

    $(document).on("click", ".deleteUserFromGroup", function (e) {
        var targetId = e.target.id;
        var chatId = $("[name = 'chatId']")[0].id;

        $.ajax({
            type: "GET",
            url: urlDeleteUserFromGroup + targetId + "&chatId=" + chatId,
            success: function (data) {
                if (data.success) {
                    toastrAlert("success", data.message);
                    placeholderElement.find('.modal').modal('hide');

                } else {
                    toastrAlert("error", data.message);
                }
            },
            error: function (e) {
                console.log(e);
            }
        })       
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