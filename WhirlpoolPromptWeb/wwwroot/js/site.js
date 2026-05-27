// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Toast de éxito: aparece al cargar la página y desaparece a los 3 segundos
document.addEventListener("DOMContentLoaded", function () {
    const toast = document.getElementById("toastSuccess");
    if (!toast) return;

    // Forzar un frame antes de agregar la clase para que la transición se anime
    requestAnimationFrame(() => {
        requestAnimationFrame(() => {
            toast.classList.add("show");
        });
    });

    // Programar la desaparición
    setTimeout(() => {
        toast.classList.remove("show");

        // Quitar el elemento del DOM cuando termine la animación de salida
        toast.addEventListener("transitionend", () => {
            toast.remove();
        }, { once: true });
    }, 3000);
});