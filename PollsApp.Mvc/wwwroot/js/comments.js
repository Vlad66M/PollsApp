"use strict";



var connection = new signalR.HubConnectionBuilder().withUrl("/commentsHub").withAutomaticReconnect().build();

connection.start().then(function () {
    console.log("Connection started comments.js");

}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveComment", function (comment) {
    console.log("ReceiveComment");
    console.log(comment);
    let currentPollId = document.getElementById("poll-id-current").value;
    if (currentPollId != comment.pollId) return;

    let text = document.createElement("p");
    text.textContent = comment.text;

    let comments = document.getElementById("comments-list");
    comments.appendChild(text);
});

document.getElementById("comments-textarea").addEventListener("keydown", function (event) {
    var key = event.key;
    if (key != "Enter" || event.shiftKey || event.ctrlKey) {
        return;
    }

    var pollId = document.getElementById("current-poll-id").value;

    var text = document.getElementById("comments-textarea").value;

    text = text.trim();
    if (text === "") {
        event.preventDefault();
        return;
    }

    connection.invoke("SendComment", pollId, text).catch(function (err) {
        return console.error(err.toString());
    });

    /*let comment = document.createElement("div");
    let commentText = document.createElement("p");
    commentText.textContent = text;
    comment.appendChild(commentText);
    document.getElementById("comments-list").appendChild(comment);*/

    event.preventDefault();

    console.log("Отправлено сообщение SendComment");
});