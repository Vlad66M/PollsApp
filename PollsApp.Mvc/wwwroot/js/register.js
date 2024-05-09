/*import Cropper from "../cropper/cropper";*/
/*import Cropper from "~/cropper/cropper.esm";*/

var image = document.getElementById("img-id");
var croppedImage = document.getElementById("img-cropped");
var input = document.getElementById("input-image");
var modal = document.getElementById("modal-crop");

var img64String;
var img64StringCropped;

var cropper;


/*var cropper = new Cropper(image, {
    aspectRatio: 0,
});*/

document.getElementById("btn-crop").addEventListener("click", function () {
    img64StringCropped = cropper.getCroppedCanvas().toDataURL();
    var parts = img64StringCropped.split(',')[1];
    document.getElementById("input-cropped-avatar").value = img64StringCropped.split(',')[1];
    console.log(img64StringCropped);
    console.log(document.getElementById("input-cropped-avatar").value);
    document.getElementById("img-cropped").src = img64StringCropped;
    modal.className = "hidden";
    img64String = "";
    image.src = "";
    input.value = '';
    cropper.destroy();
});

input.addEventListener("input", function () {
    
    cropImg();

});

function cropImg() {
    console.log("input");

    var fileList = input.files;
    console.log("fileList length:");
    console.log(fileList.length);
    if (fileList.length) {

        let fileReader = new FileReader();
        fileReader.readAsArrayBuffer(fileList[0]);
        fileReader.onload = function () {
            console.log(1);
            var binary = '';
            var bytes = new Uint8Array(fileReader.result);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            img64String = btoa(binary);
            console.log(2);

            /*let img = document.createElement("img");
            img.src = "data:image/jpeg;base64," + img64String;
            img.className = "avatar-img-visible";
            document.getElementById("modal-img-container").appendChild(img);*/
            image.src = "data:image/jpeg;base64," + img64String;
            modal.className = "modal-crop-visible";

            //cropper.destroy();
            cropper = new Cropper(image, {
                aspectRatio: 1 / 1,
            });
            console.log(3);

            /*let img = document.createElement("img");
            img.src = image.src;
            img.className = "cropped";
            document.getElementById("all-images").appendChild(img);*/
        };
    }
}





