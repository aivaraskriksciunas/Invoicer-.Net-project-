export default function AppLayout({ children }) {
    return (
        <>
            <nav>Navbar</nav>

            <div>
                {children}
            </div>
        </>
    );
}