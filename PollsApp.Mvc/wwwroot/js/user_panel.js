var showDetails = false;

document.getElementById("show-user-panel-details").addEventListener("click", (e) => {
    showDetails = !showDetails;
    if (showDetails) {
        document.getElementById("user-panel-details").className = "user-panel-details";

        let rect = document.getElementById("show-user-panel-details").getBoundingClientRect();
        /*document.getElementById("user-panel-details").style.right = (rect.right) + 'px';
        document.getElementById("user-panel-details").style.top = rect.bottom + 'px';*/
        document.getElementById("user-panel-details").style.left = (e.clientX - 50) + 'px';
        document.getElementById("user-panel-details").style.top = (e.clientY + 30)+ 'px';
        console.log(e.clientX);
        console.log(e.clientY);
    }
    else {
        document.getElementById("user-panel-details").className = "hidden";
    }

});