$(function () {

    var placeholderElement = $('#question-modal');
  
    $('button[data-toggle="ajax-questionCateg-modal"]').click(function (event) {

        $.get("/Clienti/QuestionCategories/Create").done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });

    });

    placeholderElement.on('click', '[data-save="modal-qc"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = "/Clienti/QuestionCategories/Create";
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
                toastrAlert("success", "Categorie adaugata cu succes");
                ReloadPage(2000);
            } 
        });
    })
});