import React from "react";
import { observer } from "mobx-react-lite";
import { Button } from "antd";
import FolderCreationModal from "./FolderCreationModal";

const FolderCreationModalButton: React.FC = observer(() => {
    const [isFolderCreationModalOpen, setIsFolderCreationModalOpen] =
        React.useState(false);

    const handleCreateFolderButtonClick = () => {
        setIsFolderCreationModalOpen(true);
    };

    const hideFolderCreationModal = () => {
        setIsFolderCreationModalOpen(false);
    };

    return (
        <>
            <Button onClick={handleCreateFolderButtonClick}>+</Button>
            <FolderCreationModal
                isModalOpen={isFolderCreationModalOpen}
                hideModal={hideFolderCreationModal}
            />
        </>
    );
});

export default FolderCreationModalButton;
