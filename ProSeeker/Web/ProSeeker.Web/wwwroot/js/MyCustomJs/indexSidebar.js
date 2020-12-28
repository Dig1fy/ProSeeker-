function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.querySelector('.closeNavbar').textContent = "";
}

function openNav() {
    if (document.getElementById("mySidebar").style.width == "250px") {
        document.getElementById("mySidebar").style.width = "0"
        document.querySelector('.closeNavbar').textContent = "";
    } else {
        document.getElementById("mySidebar").style.width = "250px";
        document.querySelector('.closeNavbar').textContent = "x";
    }

    // document.getElementById("mySidebar").style.width = document.getElementById("mySidebar").style.width == "250px" ? "0" : "250px";
}

function SubmitForm() {
    if (confirm('Наистина ли искате да излезете от акаунта си?') === true) {
        $('#submitForm').submit();
    }
}