$(document).ready(function () {

	// banner owl carousel
	$("#banner-area .owl-carousel").owlCarousel({
		// Specify parameters
		loop: true,
		dots: true,
		items: 1
    });

    notificationPopover();
    getNotification();

    let connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalR")
        .build();

    connection.on("displayNotification", () => {
        getNotification();
    })

    connection.start();

});

$(document).on("click", "li.notification-text", function (e) {
    // this is for dynamically generated elements, they have to be bound to some existing ones to add events on them
    var target = e.target;
    var id = target.id;
    console.log(target);

    readNotification(id, target);
});

$(document).on("mouseover", "#notification-message", function (e) {
    var target = e.target;
    $(target).fadeOut('slow');
})

function getNotification() {
    // get notifications from database for logged user
    var notificationsList = "<ul class='list-group'>";
    $.ajax({
        url: "/Clienti/Notificare/GetNotification",
        method: "GET",
        success: function (result) {
            $("#notification").html(result.count);
            var notifications = result.notificareUser;
            notifications.forEach(element => {
                notificationsList = notificationsList + "<li class='list-group-item list-group-item-dark notification-text' id='" + element.notificare.notificareId + "'>" + element.notificare.text + "</li>";
            });
            notificationsList = notificationsList + "</ul>";
            $("#notification-content").html(notificationsList);
        },
        error: function (error) {
            console.log(error);
        }
    });

}

function notificationPopover() {
    // afisare popover notificari
    $("[data-toggle='popover']").popover({
        placement: "bottom",
        content: function () {
            return $("#notification-content").html();
        },
        html: true
    });

    $('body').append("<div id='notification-content'></div>")
}

function readNotification(id, target) {

    $.ajax({
        url: "/Clienti/Notificare/ReadNotification",
        method: "GET",
        data: { notificareId: id },
        success: function () {
            getNotification();
            console.log("NOTIFICATION SEEN");
            $(target).fadeOut('slow');
        },
        error: function (error) {
            console.log(error);
        }
    });

}

// Formular Home
$('#submitMailButton').click(function (event) {

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

function Delete(url) {
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


