// create a connection to the Hub
// this connection is established after the page is displayed
// js is compiled and run

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

var _connectionId = '';

connection.on("ReceiveMessage", function (data) {
    var message = document.createElement("div")
    message.classList.add("message");

    if (data.nume === loggedUser) {
        message.classList.add("user-sender");
    } else {
        message.classList.add("user-receiver");
    }

    var header = document.createElement("header")
    header.appendChild(document.createTextNode(data.nume))

    var p = document.createElement("p")
    p.appendChild(document.createTextNode(data.text))

    var footer = document.createElement("footer")
    footer.appendChild(document.createTextNode(data.data))

    message.appendChild(header);
    message.appendChild(p);
    message.appendChild(footer);

    $(".chat-body").append(message);
})

var joinRoom = function () {
    $.ajax({
        url: "/Chat/JoinRoom/" + _connectionId + "/" + roomId,
        method: "POST",
        success: function () {
            console.log("Succes")
        },
        error: function (err) {
            console.error(err)
            console.log("eroare")
            console.log(_connectionId)
        }
    })
}

// start connection
connection.start()
    .then(function () {
        // join a group after connection established
        connection.invoke('getConnectionId')
            .then(function (connectionId) {
                _connectionId = connectionId
                joinRoom();
            })
    })
    .catch(function (err) {
        console.log(err)
    })

var form = null;

var sendMessage = function () {
    // grab the data in the form when submitting
    event.preventDefault();
    data = $('form').serialize()

    $("#message-input").val(" ")

    $.ajax({
        url: "/Chat/SendMessage",
        method: "POST",
        data: data,
        success: function () {
            console.log("Mesaj transmis")
            console.log(data)
        },
        error: function () {
            alert("Eroare la trimiterea mesajului!")
        }
    })
}


// Schimbare sageti de expansiune
var utilizatoriArea = $("#utilizatoriArea");
var grupuriArea = $("#grupuriArea");

utilizatoriArea.on("click", function () {
    var ariaExpanded = utilizatoriArea.prop("ariaExpanded");

    if (ariaExpanded === "true") {
        utilizatoriArea.html(`Utilizatori &nbsp; <i class='fas fa-angle-double-down'></i>`);
    } else {
        utilizatoriArea.html(`Utilizatori &nbsp; <i class='fas fa-angle-double-up'></i>`);
    }
});

grupuriArea.on("click", function () {
    var ariaExpanded = grupuriArea.prop("ariaExpanded");

    if (ariaExpanded === "true") {
        grupuriArea.html(`Grupuri &nbsp; <i class='fas fa-angle-double-down'></i>`);
    } else {
        grupuriArea.html(`Grupuri &nbsp; <i class='fas fa-angle-double-up'></i>`);
    }
});