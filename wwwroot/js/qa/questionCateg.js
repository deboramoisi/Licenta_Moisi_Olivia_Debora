$(function () {

    var placeholderElement = $('#question-modal');
    // la click pe buton adaugare categorie noua
    $('button[data-toggle="ajax-questionCateg-modal"]').click(function (event) {

        // cerere ajax Get - controller Create
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

        // ajax post action Create din QuestionCategories controller
        // trimitem datele serializate
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

function DeleteQuestion(url) {
    swal({
        title: "Sunteti sigur?",
        text: "In urma stergerii, datele nu vor putea fi recuperate!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((sterge) => {
        if (sterge) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        ReloadPage(1500);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function ReloadPage(timeout) {
    setTimeout(function () { window.location.reload(true); }, timeout);
}