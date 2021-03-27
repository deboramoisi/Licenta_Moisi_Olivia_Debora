var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

// addMessageToChat is passed like a delegate
connection.on('receiveMessage', addMessageToChat);

connection.start()
    .catch(error => {
        console.error(error.message);
    });

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}