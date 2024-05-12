
function showVoted(id) {
    
    //console.log("showVoted");
    let contactBtn = document.getElementById("show-voted-" + id);
    let rect = contactBtn.getBoundingClientRect();

    let votedUsersDiv = document.getElementById("voted-users-" + id);
    votedUsersDiv.className = "voted-users-visible";
    votedUsersDiv.style.left = rect.right + 'px';
    votedUsersDiv.style.top = rect.top + 'px';

    votedUsersDiv.addEventListener("mouseleave", (event) => {
        document.getElementById("voted-users-" + id).className = "hidden";
    });

    //createModal(id, rect);
}

function createModal(id, rect) {
    let menu = document.createElement("div");
    menu.id = "contact-menu-" + id;
    menu.className = "contact-menu-visible";
    let row1 = document.createElement("div");
    row1.className = "contact-menu-row";
    row1.innerHTML = "Delete messages";
    let row2 = document.createElement("div");
    row2.className = "contact-menu-row";
    row2.innerHTML = "Delete contact";
    let row3 = document.createElement("div");
    row3.className = "contact-menu-row";
    row3.innerHTML = "Block";
    menu.appendChild(row1);
    menu.appendChild(row2);
    menu.appendChild(row3);
    menu.style.left = rect.right + 'px';
    menu.style.top = rect.top + 'px';
    menu.addEventListener("mouseleave", (event) => {
        document.getElementById("contact-menu-" + id).remove();
    });
    document.body.appendChild(menu);
}

function hideVoted(id) {
    var hovered = document.querySelectorAll(':hover');
    let isOverMenu = false;
    hovered.forEach((el) => {
        if (el.className == "voted-users-visible") {
            isOverMenu = true;
        }
    });
    if (!isOverMenu) {
        //document.getElementById("contact-menu-" + id).remove();
        document.getElementById("voted-users-" + id).className = "hidden";
    }

}


function showImage(imgSrc) {
    console.log("showImage");
    document.getElementById("opened-photo").src = imgSrc;
    document.getElementById("opened-photo-container").className = "opened-photo-container-visible";
}

function hideOpenedPhoto() {
    document.getElementById("opened-photo-container").className = "opened-photo-container-hidden";
}