window.cartModal = {
    openDialog: function (id) {
        const dialog = document.getElementById(id);
        if (dialog) {
            dialog.showModal();
        }
    },

    closeDialog: function (id) {
        const dialog = document.getElementById(id);
        if (dialog) {
            dialog.close();
        }
    }
};
