const getTimeStamps = () => {
    let date = new Date();
    let thn = date.getFullYear();
    let bln = ("0" + (date.getMonth() + 1)).slice(-2);
    let tgl = ("0" + (date.getDate())).slice(-2);
    let jam = ("0" + (date.getHours())).slice(-2);
    let mnt = ("0" + (date.getMinutes())).slice(-2);
    let dtk = ("0" + (date.getSeconds())).slice(-2);
    let milliseconds = date.getMilliseconds().toString();
    return thn + bln + tgl + jam + mnt + dtk + milliseconds;
}

const generateUUID = (length=6) => {
    let result           = '';
    let characters       = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let charactersLength = characters.length;
    for ( let i = 0; i < length; i++ ) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return getTimeStamps() + result;
}
