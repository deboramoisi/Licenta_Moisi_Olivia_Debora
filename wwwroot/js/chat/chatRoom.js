$(function () {

    var placeholderElement = $('#chatRoom-modal');
    // la click pe buton adaugare categorie noua
    $('button[data-toggle="ajax-chatRoom-modal"]').click(function (event) {

        // cerere ajax Get - controller Create
        $.get("/Clienti/Chat/CreateRoomModal").done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });

    });

    placeholderElement.on('click', '[data-save="modal-cr"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = "/Clienti/Chat/CreateRoomModal";
        var dataToSend = form.serialize();

        // ajax post action Create din QuestionCategories controller
        // trimitem datele serializate
        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');

                toastrAlert("success", "Camera adaugata cu succes");
            }
        });
    })
});