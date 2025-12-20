import { useEffect } from "react";
import { useNavigate } from "react-router";

export const Callback = () => {
    const navigate = useNavigate();

    useEffect(() => {
        // Extract tokens from Url hash
        const hash = window.location.hash.substring(1);
        const params = new URLSearchParams(hash);

        const idToken = params.get('id_token');
        const accessToken = params.get('access_token');

        if(idToken && accessToken) {
            // Store tokens in localStorage
            localStorage.setItem('idToken', idToken);
            localStorage.setItem('accessToken', accessToken);

            navigate('/');
        } else {
            console.error('No tokens found in callback');
            navigate('/');
        }
    }, [navigate]);

    return <div>Loading...</div>;
}