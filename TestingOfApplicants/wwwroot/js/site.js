// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function checkMail() {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var mail = document.getElementById("mail").value;
    if (reg.test(mail) == true) {
        document.getElementById("sub-btn").disabled = false;
        document.getElementById("sub-btn").style = "background-image:linear-gradient(to right, #2492df 0%,#003265 100%)"
    }
    else {
        document.getElementById("sub-btn").disabled = true;
        document.getElementById("sub-btn").style = "background-image:linear-gradient(to right, #646464 0%,#383838 100%)";
    }
}

function onChangePasswordSignIn() {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var name = document.getElementById("authorize_name").value;
    var mail = document.getElementById("authorize_mail").value;
    var pas1 = document.getElementById("pas1").value;
    var pas2 = document.getElementById("pas2").value;
    if ((pas1 == pas2) && (name != "") && (reg.test(mail) == true) && (pas1 != "") && (pas2 != "")) {
        document.getElementById("sign_btn").disabled = false;
    }
    else {
        document.getElementById("sign_btn").disabled = true;
    }
}

function onChangePasswordLogin() {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var mail = document.getElementById("authorize_mail").value;
    var pas1 = document.getElementById("pas1").value;
    if ((reg.test(mail) == true) && (pas1 != "")) {
        document.getElementById("sign_btn").disabled = false;
    }
    else {
        document.getElementById("sign_btn").disabled = true;
    }
}

