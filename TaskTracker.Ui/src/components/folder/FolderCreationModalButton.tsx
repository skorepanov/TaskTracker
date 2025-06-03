import React from "react";
import { observer } from "mobx-react-lite";
import { Button } from "antd";
import FolderCreationModal from "./FolderCreationModal";

const FolderCreationModalButton: React.FC = observer(() => {
    const [isModalOpen, setIsModalOpen] = React.useState(false);

    const handleCreateFolderButtonClick = () => {
        setIsModalOpen(true);
    };

    const hideFolderCreationModal = () => {
        setIsModalOpen(false);
    };

    return (
        <>
            <Button onClick={handleCreateFolderButtonClick}>+</Button>
            <FolderCreationModal
                isModalOpen={isModalOpen}
                hideModal={hideFolderCreationModal}
            />
        </>
    );
});

export default FolderCreationModalButton;
