export const handleLogin = () => {
    const cognitoDomain = "https://us-east-1wmtwwn3bb.auth.us-east-1.amazoncognito.com";
    const clientId = "6hpe4kcbkvf9hogee7kg0bo1h3";
    const redirectUri = 'http://localhost:5173/callback';

    window.location.href = `https://${cognitoDomain}/login?client_id=${clientId}&response_type=token&redirect_uri=${redirectUri}`;
};