export function getUserAvatarUrl(photoUrl: string | null): string {
    if (!photoUrl)
        return "";

    if (photoUrl.startsWith('http://') || photoUrl.startsWith('https://'))
        return photoUrl.replace(/=s\d+(-c)?$/, '=s0');
        
    return `http://localhost:9000/${photoUrl}`;
}
