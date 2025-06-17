import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "./stores/RootStore";
import { LocaleProvider } from "./contexts/LocaleContext";
import AppContent from "./AppContent";

const App: React.FC = observer(() => {
    const rootStore = useStore();

    useEffect(() => {
        const fetchInitialData = async () => {
            await rootStore.fetchInitialData();
        };

        fetchInitialData().catch(console.error);
    }, [rootStore]);

    return (
        <LocaleProvider>
            <AppContent />
        </LocaleProvider>
    );
});

export default App;
