import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "./stores/RootStore";
import { SettingsProvider } from "./contexts/SettingsContext";
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
        <SettingsProvider>
            <AppContent />
        </SettingsProvider>
    );
});

export default App;
