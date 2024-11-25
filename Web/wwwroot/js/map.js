"use strict";

var mapConnection = new signalR.HubConnectionBuilder().withUrl("/hubs/map").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

mapConnection.on("ReceiveMessage", function (user, lat, lng) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} updated ${lat}, ${lng}`;
});

mapConnection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    mapConnection.invoke("UpdateLocation", user, 1, 2).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
});