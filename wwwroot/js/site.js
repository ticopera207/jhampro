// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function mostrarPopup() {
  document.getElementById("miPopup").style.display = "block";
}

function cerrarPopup() {
  document.getElementById("miPopup").style.display = "none";
}

window.onclick = function (event) {
  let popup = document.getElementById("miPopup");
  if (event.target == popup) {
    popup.style.display = "none";
  }
};
