import UserProviderWrapper from "../../components/wrappers/UserProviderWrapper";

export default function AppLayout( { children } ) {
    return (
        <UserProviderWrapper>
            <nav>Navbar</nav>

            <div>
                {children}
            </div>
        </UserProviderWrapper>
    );
}