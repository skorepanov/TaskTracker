import React from "react";
import { observer } from "mobx-react-lite";
import { Button } from "antd";
import TagCreationModal from "./TagCreationModal";

const TagCreationModalButton: React.FC = observer(() => {
    const [isModalOpen, setIsModalOpen] = React.useState(false);

    const handleCreateTagButtonClick = () => {
        setIsModalOpen(true);
    };

    const hideTagCreationModal = () => {
        setIsModalOpen(false);
    };

    return (
        <>
            <Button onClick={handleCreateTagButtonClick}>+</Button>
            <TagCreationModal
                isModalOpen={isModalOpen}
                hideModal={hideTagCreationModal}
            />
        </>
    );
});

export default TagCreationModalButton;
