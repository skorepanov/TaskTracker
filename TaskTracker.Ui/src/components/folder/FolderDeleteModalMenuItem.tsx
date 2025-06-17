import React from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "../../hooks/useTranslation";
import IFolder from "../../interfaces/IFolder";
import FolderDeleteModal from "./FolderDeleteModal";

interface IFolderDeleteModalMenuItemProps {
    folder: IFolder;
}

const FolderDeleteModalMenuItem: React.FC<IFolderDeleteModalMenuItemProps> =
    observer(props => {
        const t = useTranslation();

        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleDeleteFolderButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideFolderDeleteModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <div onClick={handleDeleteFolderButtonClick}>
                    {t("deleteFolder")}
                </div>
                <FolderDeleteModal
                    folder={props.folder}
                    isModalOpen={isModalOpen}
                    hideModal={hideFolderDeleteModal}
                />
            </>
        );
    });

export default FolderDeleteModalMenuItem;
