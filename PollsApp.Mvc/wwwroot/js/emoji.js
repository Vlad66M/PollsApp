var emojiButton = document.getElementById("emoji");
var emojiModal = document.getElementById("emojis");
var textArea = document.getElementById("comments-textarea");
var isHidden = true;

emojiButton.addEventListener("click", e => {
    if (isHidden) {
        emojiModal.className = "emojis-visible";
    }
    else {
        emojiModal.className = "emojis-hidden";
    }
    isHidden = !isHidden;

});

var allEmojis = document.querySelectorAll(".emoji-item");
var allEmojisArray = [...allEmojis];
allEmojisArray.forEach(emj => {
    emj.addEventListener("click", function () {
        textArea.value += emj.textContent;
        /*isHidden = true;
        emojiModal.className = "emojis-hidden";*/
    })
});

