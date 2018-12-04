document.addEventListener('DOMContentLoaded', () => {
    const outputArea = document.getElementById('output-panel').querySelector('p');
    let outputText = outputArea.dataset.rawtext;
    outputArea.dataset.rawtext = "";
    //Remove any HTML or JS injection attemps.
    outputText = outputText.replace( /<.*?>/g, "");

    //Trim the end to finish with a sentence or newline.
    let lastEndOfSentence = -1;
    const endChars = [".","?","!"];
    endChars.forEach( (c) => {
        let ind = outputText.lastIndexOf(c);
        if (ind > lastEndOfSentence)
            lastEndOfSentence = ind;
    });
    if (lastEndOfSentence == -1){
        ind = outputText.lastIndexOf("\n");
        if (outputText.length - ind > 50)
            lastEndOfSentence = outputText.length;
    }
    outputText = outputText.substring(0, lastEndOfSentence +1);

    //Replace newline characters with HTML line breaks.
    outputText = outputText.replace(/\n/g,"<br />");

    outputArea.innerHTML = outputText;
});
    