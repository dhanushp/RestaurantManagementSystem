// modal.js

window.initModalBehavior = () => {
    const dialog = document.querySelector('.cart-modal');

    if (!dialog) return;

    dialog.addEventListener('click', function (event) {
        if (event.target === dialog) {
            // Close modal if clicked outside the content
            document.querySelector('.cart-modal__close-btn').click(); // Trigger close button
        }
    });
};
