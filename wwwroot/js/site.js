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

const fadeInElements = document.querySelectorAll(".fade-in");

const observer = new IntersectionObserver(
  (entries) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        entry.target.classList.add("visible");
      } else {
        entry.target.classList.remove("visible");
      }
    });
  },
  {
    threshold: 0.1,
  }
);

fadeInElements.forEach((el) => observer.observe(el));

function previewImage(event) {
  const reader = new FileReader();
  reader.onload = function () {
    const output = document.getElementById("preview");
    output.src = reader.result;
  };
  reader.readAsDataURL(event.target.files[0]);
}
