"use strict";



var connection = new signalR.HubConnectionBuilder().withUrl("/commentsHub").withAutomaticReconnect().build();

document.getElementById("comments-list").scrollTop = document.getElementById("comments-list").scrollHeight;

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

    let commentItem = document.createElement("div");
    commentItem.className = "comment";

    let img = document.createElement("img");
    img.className = "comment-avatar-img";
    if (comment.userAvatar == "") {
        img.src = "https://i.pinimg.com/736x/0d/64/98/0d64989794b1a4c9d89bff571d3d5842.jpg";
    }
    else {
        img.src = "data:image/jpeg;base64," + comment.userAvatar;
    }

    let userName = document.createElement("p");
    userName.className = "comment-user-name";
    userName.innerHTML = comment.userName + ": ";

    let text = document.createElement("p");
    text.textContent = comment.text;
    text.className = "comment-text";

    commentItem.appendChild(img);
    commentItem.appendChild(userName);
    commentItem.appendChild(text);

    let comments = document.getElementById("comments-list");
    comments.appendChild(commentItem);

    comments.scrollTop = comments.scrollHeight;

    document.getElementById("comments-textarea").value = "";
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