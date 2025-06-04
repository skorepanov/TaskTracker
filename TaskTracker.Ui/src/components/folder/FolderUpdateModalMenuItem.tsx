import React from "react";
import { observer } from "mobx-react-lite";
import IFolder from "../../interfaces/IFolder";
import FolderUpdateModal from "./FolderUpdateModal";

interface IFolderUpdateMenuItemProps {
    folder: IFolder;
}

const FolderUpdateModalMenuItem: React.FC<IFolderUpdateMenuItemProps> =
    observer(props => {
        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleUpdateFolderButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideFolderUpdateModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <div onClick={handleUpdateFolderButtonClick}>
                    Редактировать папку
                </div>
                <FolderUpdateModal
                    folder={props.folder}
                    isModalOpen={isModalOpen}
                    hideModal={hideFolderUpdateModal}
                />
            </>
        );
    });

export default FolderUpdateModalMenuItem;
