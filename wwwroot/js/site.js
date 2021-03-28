// Initialize the owl carousel using JQuery

$(document).ready(function () {

	// banner owl carousel
	$("#banner-area .owl-carousel").owlCarousel({
		// Specify parameters
		loop: true,
		dots: true,
		items: 1
	});

});

// Formular Home
$('#submitButton').click(function (event) {

    var form = $('#sendFormHome');

    $.ajax({
        type: "POST",
        url: "/Clienti/Home/SendForm",
        data: form.serialize(),
        success: function (data) {
            if (data.success) {
                toastrAlert("success", data.message)
                $("[name = 'Mesaj']").val(null);
                $("[name = 'Subiect']").val(null);
                $("[name = 'Email']").val(null);
                $("[name = 'Nume']").val(null);
            } else {
                toastrAlert("error", data.message)
            }
        }
    });

    event.preventDefault();   

})

function isEmpty(array) {
    return (array.length === 0) ? true : false;
}

function getExtensie(data) {
    var icon = "";
    if (data.includes("pdf")) {
        icon = "far fa-file-pdf";
    } else if (data.includes("docx") || data.includes("doc")) {
        icon = "far fa-file-word";
    } else if (data.includes("xlsx") || data.includes("csv")) {
        icon = "far fa-file-excel";
    } else if (data.includes("png") || data.includes("jpg") || data.includes("jpeg") || data.includes("img")) {
        icon = "far fa-images";
    }
    return icon;
}

function toastrAlert(type, message) {
    // toastr pentru succes
    toastr[type](message)

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}



function Delete2(url) {
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
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}