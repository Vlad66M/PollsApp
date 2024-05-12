var opNumber = 0;
var hasEndDateCkbx = document.getElementById("ckbx-has-end-date");
var hasEndDate = hasEndDateCkbx.checked;
var datepicker = document.getElementById("datepicker");

try {
    $(function () {
        $("#datepicker").datepicker();
    });
}
catch (e) { }



document.getElementById("add-option").addEventListener("click", () => {
    document.getElementById("add-option").disabled = true;
    opNumber++;
    let optionItem = document.createElement("div");
    optionItem.className = "option-item-create";

    let optionText = document.createElement("input");
    optionText.className = "poll-option-input";
    optionText.setAttribute('type', 'text');
    let opTextName = 'options[' + opNumber + '].text';
    optionText.setAttribute('name', 'options');
    optionItem.appendChild(optionText);

    let optionAudioVal = document.createElement("input");
    optionAudioVal.id = "audio" + opNumber + 'val';
    optionAudioVal.className = "hidden";
    optionAudioVal.setAttribute('type', 'text');
    optionAudioVal.value = "";
    let opAudioName = 'options[' + opNumber + '].audio';
    optionAudioVal.setAttribute('name', 'audios');
    optionItem.appendChild(optionAudioVal);

    let optionPhotoVal = document.createElement("input");
    optionPhotoVal.id = "photo" + opNumber + 'val';
    optionPhotoVal.className = "hidden";
    optionPhotoVal.setAttribute('type', 'text');
    optionPhotoVal.value = "";
    let opPhotoName = 'photos[' + opNumber + '].audio';
    optionPhotoVal.setAttribute('name', 'photos');
    optionItem.appendChild(optionPhotoVal);

    let optionAudio = document.createElement("input");
    optionAudio.id = "audio" + opNumber;
    optionAudio.className = "label-hidden";
    optionAudio.setAttribute('type', 'file');
    optionAudio.setAttribute('accept', 'audio/*');
    optionItem.appendChild(optionAudio);
    /*let br1 = document.createElement("br");
    optionItem.appendChild(br1);*/
    optionAudio.addEventListener("change", (e) => {

        setTimeout(() => {
            var filelist = document.getElementById("audio" + opNumber).files;
            let t = e.target;
            console.log("id: " + t.id);
            if (filelist[0]) {
                console.log("aaa");
                let fileReader = new FileReader();
                fileReader.readAsArrayBuffer(filelist[0]);
                fileReader.onload = function () {
                    console.log(1);
                    var binary = '';
                    var bytes = new Uint8Array(fileReader.result);
                    var len = bytes.byteLength;
                    for (var i = 0; i < len; i++) {
                        binary += String.fromCharCode(bytes[i]);
                    }

                    var audio64String = btoa(binary);
                    document.getElementById("audio" + opNumber + 'val').value = audio64String;
                    console.log("filelist" + audio64String);

                    var sound = document.createElement('audio');
                    sound.id = 'audio-player';
                    sound.controls = 'controls';
                    sound.src = "data:audio/mp3;base64," + audio64String;
                    sound.type = 'audio/mpeg';
                    //document.getElementById("options-container").appendChild(sound);
                    //t.parentNode.appendChild(sound);
                    t.parentNode.insertBefore(sound, t);
                    t.className = "hidden";
                    document.getElementById("audio" + opNumber + "label").className = "hidden";
                };
            }
        }, 1);



    });

    let audioLabel = document.createElement("label");
    audioLabel.htmlFor = "audio" + opNumber;
    audioLabel.innerHTML = "&#127911;";
    audioLabel.id = "audio" + opNumber + "label";
    audioLabel.className = "option-label";

    let optionPhoto = document.createElement("input");
    optionPhoto.id = "photo" + opNumber;
    optionPhoto.className = "label-hidden";
    optionPhoto.setAttribute('type', 'file');
    optionPhoto.setAttribute('accept', 'image/*');
    optionItem.appendChild(optionPhoto);
    /*let br2 = document.createElement("br");
    optionItem.appendChild(br2);*/
    optionPhoto.addEventListener("change", (e) => {

        setTimeout(() => {
            var filelist = document.getElementById("photo" + opNumber).files;
            let t = e.target;
            console.log("id: " + t.id);
            if (filelist[0]) {
                console.log("aaa");
                let fileReader = new FileReader();
                fileReader.readAsArrayBuffer(filelist[0]);
                fileReader.onload = function () {
                    console.log(1);
                    var binary = '';
                    var bytes = new Uint8Array(fileReader.result);
                    var len = bytes.byteLength;
                    for (var i = 0; i < len; i++) {
                        binary += String.fromCharCode(bytes[i]);
                    }

                    var photo64String = btoa(binary);
                    document.getElementById("photo" + opNumber + 'val').value = photo64String;
                    console.log("filelist" + photo64String);

                    var img = document.createElement('img');
                    img.className = "option-photo";
                    img.src = "data:image/jpeg;base64," + photo64String;
                    //document.getElementById("options-container").appendChild(img);
                    //t.parentNode.appendChild(img);
                    t.parentNode.insertBefore(img, t);
                    t.className = "hidden";
                    document.getElementById("photo" + opNumber + "label").className = "hidden";

                };
            }
        }, 1);



    });

    let photoLabel = document.createElement("label");
    photoLabel.htmlFor = "photo" + opNumber;
    photoLabel.innerHTML = "&#128247;";
    photoLabel.id = "photo" + opNumber + "label";
    photoLabel.className = "option-label";

    optionItem.appendChild(audioLabel);
    optionItem.appendChild(photoLabel);


    let removeButton = document.createElement("div");
    removeButton.className = "remove-option";
    optionItem.appendChild(removeButton);
    removeButton.innerHTML = "&#10060;";
    removeButton.addEventListener("click", (e) => {
        console.log(e);
        opNumber--;
        return e.currentTarget.parentNode.remove();
    });



    document.getElementById("options-container").appendChild(optionItem);
    document.getElementById("add-option").disabled = false;
});

hasEndDateCkbx.addEventListener("change", () => {
    setTimeout(() => {
        console.log("ckbx");
        console.log(hasEndDate);
        hasEndDate = !hasEndDate;
        hasEndDateCkbx.value = hasEndDate;
        console.log(hasEndDate);
        if (hasEndDate) {
            datepicker.className = "datepicker-visible";
        }
        else {
            datepicker.className = "datepicker-hidden";
        }
    }, 100);

});


//

