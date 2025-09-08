/** Tiny auth helpers for Data Router loaders. */
export const isAuthed = (): boolean => {
    const t = localStorage.getItem("access_token");
    return !!t && t.length > 10;
};

/** Guard for /app subtree. */
export const protectedLoader = () => {
    if (!isAuthed()) {
        const r = new URLSearchParams({ r: location.pathname || "/app" });
        throw new Response(null, { status: 302, headers: { Location: `/login?${r}` } });
    }
    return null;
};
