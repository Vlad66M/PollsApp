
var buttons = document.querySelectorAll(".delete-btn");
buttons.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        let confirmed = confirm("delete poll?");
        if (confirmed) {
            return e.currentTarget.parentNode.submit();
        }
    });
});
