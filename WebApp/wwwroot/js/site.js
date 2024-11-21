window.preventDefaultBehavior = (event) => {
    if (event && event.preventDefault) {
        event.preventDefault(); // Prevent default behavior
    } else {
        console.warn("preventDefaultBehavior called without a valid event object.");
    }
};
