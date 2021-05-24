var calendar;

$(document).ready(function () {
    InitializeCalendar();
});

function InitializeCalendar() {
    try {
        // initializare calendar
        var calendarElement = document.getElementById('calendar');
        calendar = new FullCalendar.Calendar(calendarElement, {
            initialView: 'dayGridMonth',
            timeZone: 'UTC',
            headerToolbar:
            {
                left: 'prev,next,today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            selectable: true,
            select: function (event) {
                onShowModal(event, null);
            },
            events: function (fetchInfo, successCallback, failureCallback) {
                $.ajax({
                    url: "/Clienti/CereriDocumente/GetCereriCalendar",
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (data) {
                        var events = [];
                        $.each(data.cereri, function (i, data) {
                            events.push({
                                title: data.denumireCerere,
                                description: data.denumireClient,
                                start: data.dataStart,
                                backgroundColor: data.resolved ? "#228B22" : "#8B0000",
                                borderColor: "FFFAF0",
                                textColor: "white",
                                id: data.cerereDocumentId
                            });
                        })
                      
                        successCallback(events);
                    },
                    error: function (data) {
                        console.log(data);
                        toastrAlert("error", data.message);
                    }
                });         

            },
            eventClick: function (information) {
                getCerereDetailsByCerereId(information.event);
            }
        });

        calendar.render();

    }
    catch (e) {
        alert(e);
    }
}

function onShowModal(cerereDetails, inEditingMode) {
    if (inEditingMode != null) {

        $('[name = "CerereDocumentId"]').val(cerereDetails.cerereDocumentId);
        $('[name = "SalariatId"]').val(cerereDetails.salariatId);
        $('[name = "TipCerereId"]').val(cerereDetails.tipCerereId);
        $('[name = "DataStart"]').val(cerereDetails.dataStart);
        $('[name = "DenumireClient"]').val(cerereDetails.denumireClient);

        var isAdmin = $("#adminInput").val();
        if (isAdmin) {
            $("#incarcaDoc").html(`<a class="btn btn-primary" href="/Admin/Documents/Create/${cerereDetails.cerereDocumentId}">Incarca document</a>`);
        }
        console.log($("#adminInput").val());

    } else {
        $('[name = "DataStart"]').val(cerereDetails.startStr);
    }
    $("#cerereDocument").modal("show");

}

function onCloseModal() {
    $("#cerereDocument").modal("hide");
}

function onSubmitCerere() {
    var URL = $("form")[1].action;
   
    var data = {
        CerereDocumentId: parseInt($('[name = "CerereDocumentId"]').val()),
        ApplicationUserId: $('[name = "ApplicationUserId"]').val(),
        DenumireClient: $('[name = "DenumireClient"]').val(),
        TipCerereId: parseInt($('[name = "TipCerereId"]').val()),
        SalariatId: parseInt($('[name = "SalariatId"]').val()),
        DataStart: $('[name = "DataStart"]').val(),
    };

    $.ajax({
        url: URL,
        type: 'POST',
        data: data,
        success: function (data) {
            calendar.refetchEvents();
            onCloseModal();
            toastrAlert("success", data.message);
        },
        error: function (data) {
            toastrAlert("error", data);
        }
    });
}

function onDeleteCerere() {

    var id = parseInt($('[name = "CerereDocumentId"]').val());

    $.ajax({
        url: "/Clienti/CereriDocumente/DeleteCerere/" + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (data) {
            if (data.success) {
                calendar.refetchEvents();
                onCloseModal();
                toastrAlert("success", data.message);
            } else {
                toastrAlert("error", data.message);
            }
        },
        error: function (data) {
            toastrAlert("error", data);
        }
    });
}

function getCerereDetailsByCerereId(information) {
    console.log(information);
    $.ajax({
        url: "/Clienti/CereriDocumente/CerereDocumentById/" + information.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (data) {

            if (data.success) {
                console.log(data.cerere.denumireClient);
                onShowModal(data.cerere, true);
            }
            
        },
        error: function (data) {
            console.log(data);
            toastrAlert("error", data);
        }
    });    
    
}