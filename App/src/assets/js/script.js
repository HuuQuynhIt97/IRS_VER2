
// function texter(str, x, y, ctx) {
//     const colors = ["#f13660", "#d47b0b", "#de784a", "#8a3866"];


//     for (var i = 0; i <= str.length; ++i) {
//         const random = Math.floor(Math.random() * colors.length);
//         var ch = str.charAt(i);
//         ctx.fillStyle = colors[random];
//         ctx.fillText(str.charAt(i), x, y);
//         x += ctx.measureText(ch).width;
//     }
// }
// function createCaptcha() {
//     var code;
//     //clear the contents of captcha div first
//     document.getElementById('captcha').innerHTML = "";
//     var charsArray =
//         "0123456789";
//     var lengthOtp = 5;
//     var captcha = [];
//     for (var i = 0; i < lengthOtp; i++) {
//         //below code will not allow Repetition of Characters
//         var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
//         if (captcha.indexOf(charsArray[index]) == -1)
//             captcha.push(charsArray[index]);
//         else i--;
//     }
//     var canv = document.createElement("canvas");
//     canv.id = "captcha";
//     canv.width = 200;
//     canv.height = 65;
//     var ctx = canv.getContext("2d");
//     ctx.font = "30px Comic Sans MS";
//     // ctx.strokeText(captcha.join(""), 0, 30);
//     texter(captcha.join(" "), 0, 40, ctx)
//     //storing captcha so that can validate you can save it somewhere else according to your specific requirements
//     code = captcha.join("");
//     document.getElementById("captcha").appendChild(canv); // adds the canvas to the body element
// }
// function validateCaptcha(code) {
//     event.preventDefault();
//     if (document.getElementById("cpatchaTextBox").value == code) {
//         alert("Valid Captcha")
//     } else {
//         alert("Invalid Captcha. try Again");
//         createCaptcha();
//     }
// }




$(function () {
    console.log("ready!");
    $('.nav > .sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('.sidebar-toggle').toggleClass('active');
        $('.menu-collapse').toggleClass('active');
        $('.sidebar.slimScroll').toggleClass('active');
    });

    $('a.toggle').on('click', function (e) {
        e.preventDefault();
        $(this).toggleClass('active');
    });
    $('.nav > .dropdown .sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('.dropdown-menu.dropdown-menu-right.navbar-dropdown').toggleClass('show');
    });
});
