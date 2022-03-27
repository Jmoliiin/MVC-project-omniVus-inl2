function minValue(str, minValue) {
    let strLengt = str.trim();
    let expression = '^.{' + minValue + ',35}$';
    const r = new RegExp(expression);

    if (r.test(strLengt))
        return true
}





function validateEmail(str) {
    const regEx = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (regEx.test(str))
        return true

}




function button() {
    const button = document.querySelector('.submit');
    button.disabled = true;
    const exist = document.querySelectorAll('.button-off')

    if (exist.length > 0) {
        button.disabled = true
    } else {
        button.disabled = false
    }
}



button();

function onSubmit() {
    alert("submitted")
}
const valid = document.querySelectorAll('.validation')
valid.forEach(element => {
    if (element.type === "text" || element.type === "textarea") {
        element.addEventListener("keyup", function (e) {
            if (!minValue(e.target.value, "2")) {
                e.target.classList.add("button-off")
                e.target.classList.add("is-invalid")
                button();
            } else {
                e.target.classList.remove("is-invalid")
                e.target.classList.add("is-valid")
                e.target.classList.remove("button-off")
                button();
            }
        })
    }
    if (element.type === "email") {
        element.addEventListener("keyup", function (e) {
            if (!validateEmail(e.target.value)) {
                e.target.classList.add("button-off")
                e.target.classList.add("is-invalid")
                button();
            } else {
                e.target.classList.remove("is-invalid")
                e.target.classList.add("is-valid")
                e.target.classList.remove("button-off")
                button();
            }
        })
    }
})