window.preventDefaultBehavior = (event) => {
    if (event && event.preventDefault) {
        event.preventDefault();
    } else {
        console.warn("preventDefaultBehavior called without a valid event object.");
    }
};
