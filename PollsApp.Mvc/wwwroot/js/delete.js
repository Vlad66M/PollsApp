
var buttons = document.querySelectorAll(".delete-btn");
buttons.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        let confirmed = confirm("������� �����?");
        if (confirmed) {
            return e.currentTarget.parentNode.submit();
        }
    });
});
