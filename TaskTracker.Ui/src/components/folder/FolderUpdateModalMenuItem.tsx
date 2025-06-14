import React from "react";
import { observer } from "mobx-react-lite";
import { useTranslation } from "../../hooks/useTranslation";
import IFolder from "../../interfaces/IFolder";
import FolderUpdateModal from "./FolderUpdateModal";

interface IFolderUpdateMenuItemProps {
    folder: IFolder;
}

const FolderUpdateModalMenuItem: React.FC<IFolderUpdateMenuItemProps> =
    observer(props => {
        const t = useTranslation();

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
                    {t("updateFolder")}
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
