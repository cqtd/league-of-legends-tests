mergeInto(LibraryManager.library, {

    OpenWindow: function(url) {
        url = Pointer_stringify(url);
        
        console.log(url);
        window.open(url, '_blank');
    },
});