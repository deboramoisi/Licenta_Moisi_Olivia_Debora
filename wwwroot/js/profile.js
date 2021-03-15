$(function () {
    var PlaceHolderElementModal = $('#PlaceHolderHereModal');
     // On Click pe butonul de adaugare
    $('button[data-toggle="ajax-profile-modal"]').click(function (event) {
        // url e metoda care se apeleaza din controller
        var url = 'Manage/Index?handler=UpdateImage';
        $.get(url).done(function (data) {
            PlaceHolderElementModal.html(data);
            alert(url);
            // afisare fereastra modala
            PlaceHolderElementModal.find('.modal').modal('show');
        })
    })

    // Cand se da click pe "Save" button
    PlaceHolderElementModal.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            // Dupa adaugare date in tabel, inchidem fereastra modala
            PlaceHolderElementModal.find('.modal').modal('hide');
        })

    })

})